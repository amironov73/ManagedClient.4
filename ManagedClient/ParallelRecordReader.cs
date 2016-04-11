/* ParallelRecordReader.cs
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Считывание записей с сервера в
    /// несколько потоков.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ParallelRecordReader
        : IEnumerable<IrbisRecord>,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Степень параллелизма.
        /// </summary>
        public int Parallelism { get; private set; }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Признак окончания.
        /// </summary>
        public bool Stop { get { return _AllDone(); } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ParallelRecordReader ()
        {
            int parallelism = Environment.ProcessorCount;
            string connectionString
                = IrbisUtilities.GetConnectionString();
            int[] mfnList = _GetMfnList(connectionString);
            _Run
                (
                    parallelism,
                    connectionString,
                    mfnList
                );
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism
            )
        {
            string connectionString 
                = IrbisUtilities.GetConnectionString();
            int[] mfnList = _GetMfnList(connectionString);
            _Run
                (
                    parallelism,
                    connectionString,
                    mfnList
                );
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism,
                string connectionString
            )
        {
            int[] mfnList = _GetMfnList(connectionString);
            _Run
                (
                    parallelism,
                    connectionString,
                    mfnList
                );
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism,
                string connectionString,
                int[] mfnList
            )
        {
            _Run
                (
                    parallelism,
                    connectionString,
                    mfnList
                );
        }

        #endregion

        #region Private members

        private Task[] _tasks;

        private int[] _mfnList;

        private ConcurrentQueue<IrbisRecord> _queue;

        private AutoResetEvent _event;

        private object _lock;

        private int[] _GetMfnList
            (
                string connectionString
            )
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            using (ManagedClient64 client = new ManagedClient64())
            {
                client.ParseConnectionString(connectionString);
                client.Connect();
                int maxMfn = client.GetMaxMfn() - 1;
                if (maxMfn <= 0)
                {
                    throw new ApplicationException("MaxMFN=0");
                }
                int[] result = Enumerable.Range(1, maxMfn).ToArray();
                return result;
            }
        }

        private void _Run
            (
                int parallelism,
                string connectionString,
                int[] mfnList
            )
        {
            int maxParallelism = Environment.ProcessorCount;
            if ((parallelism < 1)
                || (parallelism > maxParallelism))
            {
                parallelism = maxParallelism;
            }
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (ReferenceEquals(mfnList, null))
            {
                throw new ArgumentNullException("mfnList");
            }

            ConnectionString = connectionString;
            parallelism = Math.Min(mfnList.Length / 100, parallelism);
            Parallelism = parallelism;

            _queue = new ConcurrentQueue<IrbisRecord>();
            _event = new AutoResetEvent(false);
            _lock = new object();

            _tasks = new Task[parallelism];
            int[][] chunks = Utilities.SplitArray(mfnList, parallelism);
            for (int i = 0; i < parallelism; i++)
            {
                Task task = new Task
                    (
                        _Worker,
                        chunks[i]
                    );
                _tasks[i] = task;
            }
            foreach (Task task in _tasks)
            {
                Thread.Sleep(50);
                task.Start();
            }
            
        }

        private void _Worker
            (
                object state
            )
        {
            int[] chunk = (int[]) state;
            //foreach (int mfn in chunk)
            //{
            //    IrbisRecord record = new IrbisRecord{Mfn = mfn};
            //    _PutRecord(record);
            //}
            //_event.Set();

            using (ManagedClient64 client = new ManagedClient64())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();
                BatchRecordReader batch = new BatchRecordReader
                    (
                        client,
                        chunk
                    );
                foreach (IrbisRecord record in batch)
                {
                    _PutRecord(record);
                }
            }
            _event.Set();
        }

        private void _PutRecord
            (
                IrbisRecord record
            )
        {
            _queue.Enqueue(record);
            _event.Set();
        }

        private bool _AllDone()
        {
            return _queue.IsEmpty
                && _tasks.All(t => t.IsCompleted);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Ожидание окончания.
        /// </summary>
        public void WaitAll()
        {
            Task.WaitAll(_tasks);
        }

        #endregion

        #region IEnumerable<T> members

        public IEnumerator<IrbisRecord> GetEnumerator()
        {
            while (true)
            {
                if (Stop)
                {
                    yield break;
                }

                IrbisRecord record;
                while (_queue.TryDequeue(out record))
                {
                    yield return record;
                }
                _event.Reset();

                _event.WaitOne(10);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Считываем все записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IrbisRecord[] ReadAll()
        {
            List<IrbisRecord> result = new List<IrbisRecord>();

            foreach (IrbisRecord record in this)
            {
                result.Add(record);
            }

            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            _event.Dispose();
        }

        #endregion
    }
}

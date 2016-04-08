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
        public bool Stop { get { return _stop; } }

        #endregion

        #region Construction

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
            int maxParallelism = Environment.ProcessorCount;
            if ((parallelism < 1)
                || (parallelism>maxParallelism))
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

            parallelism = Math.Max(mfnList.Length/100, parallelism);

            Parallelism = parallelism;

            _queue = new ConcurrentQueue<IrbisRecord>();
            _event = new AutoResetEvent(false);
            _lock = new object();

            _tasks = new Task[parallelism];
            for (int i = 0; i < parallelism; i++)
            {
                Task task = Task.Factory.StartNew(_Worker);
                _tasks[i] = task;
            }
        }

        #endregion

        #region Private members

        private Task[] _tasks;

        private int[] _mfnList;

        private readonly ConcurrentQueue<IrbisRecord> _queue;

        private AutoResetEvent _event;

        private object _lock;

        private bool _stop;

        private void _Worker
            (
            )
        {
            ManagedClient64 client = new ManagedClient64();
            client.ParseConnectionString(ConnectionString);
            client.Connect();


        }

        private void _PutRecord
            (
                IrbisRecord record
            )
        {
            _queue.Enqueue(record);
            _event.Set();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Ожидание окончания.
        /// </summary>
        public void WaitAll()
        {

        }

        #endregion

        #region IEnumerable<T> members

        public IEnumerator<IrbisRecord> GetEnumerator()
        {
            while (true)
            {
                if (_stop)
                {
                    yield break;
                }

                if (!_event.WaitOne())
                {
                    yield break;
                }

                IrbisRecord record;
                if (_queue.TryDequeue(out record))
                {
                    yield return record;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

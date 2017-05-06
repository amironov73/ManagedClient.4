// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ParallelFormatter.cs -- форматирование записей в несколько потоков.
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Форматирование записей в несколько потоков.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ParallelFormatter
        : IEnumerable<string>,
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

        /// <summary>
        /// Используемый формат.
        /// </summary>
        public string Format { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelFormatter
            (
                int parallelism,
                [NotNull] string connectionString,
                [NotNull] int[] mfnList,
                [NotNull] string format
            )
        {
        }

        #endregion

        #region Private members

        private Task[] _tasks;

        private ConcurrentQueue<string> _queue;

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
            int maxParallelism = Environment.ProcessorCount - 1;
            if (parallelism > maxParallelism)
            {
                parallelism = maxParallelism;
            }
            if (parallelism < 1)
            {
                parallelism = 1;
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

            _queue = new ConcurrentQueue<string>();
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
            int[] chunk = (int[])state;

            using (ManagedClient64 client = new ManagedClient64())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                BatchRecordFormatter batch = new BatchRecordFormatter
                    (
                        client,
                        chunk,
                        Format
                    );
                foreach (string line in batch)
                {
                    _PutLine(line);
                }

            }
            _event.Set();
        }

        private void _PutLine
            (
                string line
            )
        {
            _queue.Enqueue(line);
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
        /// Форматирование всех записей.
        /// </summary>
        [NotNull]
        public string[] FormatAll()
        {
            List<string> result = new List<string>();

            foreach (string line in this)
            {
                result.Add(line);
            }

            return result.ToArray();
        }

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<string> GetEnumerator()
        {
            while (true)
            {
                if (Stop)
                {
                    yield break;
                }

                string line;
                while (_queue.TryDequeue(out line))
                {
                    yield return line;
                }
                _event.Reset();

                _event.WaitOne(10);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _event.Dispose();
            foreach (Task task in _tasks)
            {
                task.Dispose();
            }
        }

        #endregion
    }
}

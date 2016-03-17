/* BatchRecordReader
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Считывание записей с сервера большими порциями
    /// для увеличения производительности.
    /// Сами записи отдаются по одной.
    /// </summary>
    [MoonSharpUserData]
    public sealed class BatchRecordReader
        : IEnumerable<IrbisRecord>
    {
        #region Constants

        /// <summary>
        /// Размер порции по умолчанию.
        /// </summary>
        public const int DefaultBatchSize = 1000;

        #endregion

        #region Events

	    public event EventHandler BatchRead;

        public event EventHandler<ExceptionEventArgs<Exception>> Exception;

	    #endregion

        #region Properties

        [DefaultValue(DefaultBatchSize)]
        public int BatchSize { get; private set; }

        public ManagedClient64 Client { get; private set; }

        #endregion

        #region Construction

        public BatchRecordReader
            (
                ManagedClient64 client
            )
            : this 
                (
                    client,
                    DefaultBatchSize
                )
        {
        }

        public BatchRecordReader
            (
                ManagedClient64 client,
                int batchSize
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (batchSize < 1)
            {
                throw new ArgumentOutOfRangeException("batchSize");
            }
            Client = client;
            BatchSize = batchSize;
            IEnumerable<int> range = Enumerable.Range(1, Client.GetMaxMfn());
            _InitializePackages(range);
        }

        public BatchRecordReader
            (
                ManagedClient64 client,
                string format,
                params object[] args
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }
            Client = client;
            BatchSize = DefaultBatchSize;
            int[] found = Client.Search(format, args);
            _InitializePackages(found);
        }

        public BatchRecordReader
            (
                ManagedClient64 client,
                int batchSize,
                IEnumerable<int> range
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (batchSize < 1)
            {
                throw new ArgumentOutOfRangeException("batchSize");
            }
            if (ReferenceEquals(range, null))
            {
                throw new ArgumentNullException("range");
            }
            Client = client;
            BatchSize = batchSize;
            _InitializePackages(range);
        }

        public BatchRecordReader
            (
                ManagedClient64 client,
                IEnumerable<int> range
            )
            : this
                (
                    client,
                    DefaultBatchSize,
                    range
                )
        {
        }

        #endregion

        #region Private members

        private IEnumerable<int[]> _packages;

        private void _InitializePackages
            (
                IEnumerable<int> range
            )
        {
            _packages = range.Slice(BatchSize);
        }

	    private void _OnBatchRead ()
	    {
		    EventHandler handler = BatchRead;
		    if (handler != null)
		    {
			    handler ( this, EventArgs.Empty );
		    }
	    }

        private void _OnException
            (
                Exception ex
            )
        {
            EventHandler<ExceptionEventArgs<Exception>> handler = Exception;
            if (handler != null)
            {
                ExceptionEventArgs<Exception> args = new ExceptionEventArgs<Exception>(ex);
                handler(this, args);
            }
        }

        #endregion

        #region IEnumerable members

        public IEnumerator<IrbisRecord> GetEnumerator()
        {
            foreach (int[] package in _packages)
            {
                IrbisRecord[] records = null;
                try
                {
                    records = Client.ReadRecords(package);
                    _OnBatchRead();
                }
                catch (Exception ex)
                {
                    _OnException(ex);
                }
                if (records != null)
                {
                    foreach (IrbisRecord record in records)
                    {
                        yield return record;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

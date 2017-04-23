// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BatchRecordFormatter -- форматирование записей большими порциями.
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Форматирование записей с сервера большими порциями
    /// для увеличения производительности.
    /// Сами записи отдаются по одной.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BatchRecordFormatter
        : IEnumerable<string>
    {
        #region Constants

        /// <summary>
        /// Размер порции по умолчанию.
        /// </summary>
        public const int DefaultBatchSize = 1000;

        #endregion

        #region Events

        /// <summary>
        /// Вызывается во время считывания очередной порции.
        /// </summary>
        public event EventHandler BatchRead;

        /// <summary>
        /// Вызывается при возникновении исключения.
        /// </summary>
        public event EventHandler<ExceptionEventArgs<Exception>> Exception;

        #endregion

        #region Properties

        /// <summary>
        /// Размер порции.
        /// </summary>
        [DefaultValue(DefaultBatchSize)]
        public int BatchSize { get; private set; }

        /// <summary>
        /// ИРБИС-клиент.
        /// </summary>
        public ManagedClient64 Client { get; private set; }

        /// <summary>
        /// Формат.
        /// </summary>
        public string Format { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BatchRecordFormatter
            (
                [NotNull] ManagedClient64 client,
                [NotNull] string format
            )
            : this
                (
                    client,
                    DefaultBatchSize,
                    format
                )
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BatchRecordFormatter
            (
                [NotNull] ManagedClient64 client,
                int batchSize,
                [NotNull] string format
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
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }
            Client = client;
            BatchSize = batchSize;
            Format = format;
            IEnumerable<int> range = Enumerable.Range(1, Client.GetMaxMfn());
            _InitializePackages(range);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BatchRecordFormatter
            (
                [NotNull] ManagedClient64 client,
                [NotNull] string format,
                [NotNull] string query,
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
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }
            Client = client;
            BatchSize = DefaultBatchSize;
            Format = format;
            int[] found = Client.Search(query, args);
            _InitializePackages(found);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BatchRecordFormatter
            (
                [NotNull] ManagedClient64 client,
                int batchSize,
                [NotNull] IEnumerable<int> range,
                [NotNull] string format
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
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }
            Client = client;
            BatchSize = batchSize;
            Format = format;
            _InitializePackages(range);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BatchRecordFormatter
            (
                [NotNull] ManagedClient64 client,
                [NotNull] IEnumerable<int> range,
                [NotNull] string format
            )
            : this
                (
                    client,
                    DefaultBatchSize,
                    range,
                    format
                )
        {
        }

        #endregion

        #region Private members

        private IEnumerable<int[]> _packages;

        private void _InitializePackages
            (
                [NotNull] IEnumerable<int> range
            )
        {
            _packages = range.Slice(BatchSize);
        }

        private void _OnBatchRead()
        {
            EventHandler handler = BatchRead;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
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

        #region Public methods

        /// <summary>
        /// Считывает все записи сразу.
        /// </summary>
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

        #region IEnumerable members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (int[] package in _packages)
            {
                string[] lines = null;
                try
                {
                    lines = Client.FormatRecords
                        (
                            Format,
                            package
                        );
                    _OnBatchRead();
                }
                catch (Exception ex)
                {
                    _OnException(ex);
                }
                if (lines != null)
                {
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            string tmp = line.Trim();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                yield return tmp;
                            }
                        }
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

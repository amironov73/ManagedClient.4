// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordBuffer.cs -- накопление записей перед отправкой
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Накапливает записи и отправляет их на сервер пакетами.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordBuffer
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Raised on batch record write.
        /// </summary>
        public event EventHandler BatchWrite;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int Capacity { get; private set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database
        {
            get { return _database; }
            set
            {
                lock (_syncRoot)
                {
                    _database = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RecordBuffer" /> is actualize.
        /// </summary>
        /// <value>
        ///   <c>true</c> if actualize; otherwise, <c>false</c>.
        /// </value>
        public bool Actualize { get; set; }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public ManagedClient64 Client { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordBuffer
            (
                [NotNull] ManagedClient64 client
            )
            : this(client, 10)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordBuffer
            (
                [NotNull] ManagedClient64 client,
                int capacity
            )
        {
            _syncRoot = new object();
            _records = new List<IrbisRecord>();

            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            Capacity = capacity;

            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            Client = client;
        }

        #endregion

        #region Private members

        private readonly object _syncRoot;

        private readonly List<IrbisRecord> _records;

        private string _database;

        private void _OnBatchWrite()
        {
            EventHandler handler = BatchWrite;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Appends the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <exception cref="System.ArgumentNullException">record</exception>
        public void Append
            (
                [NotNull] IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            lock (_syncRoot)
            {
                if (_records.Count >= Capacity)
                {
                    Flush();
                }
                _records.Add(record);
                if (_records.Count >= Capacity)
                {
                    Flush();
                }
            }
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
            lock (_syncRoot)
            {
                if (_records.Count != 0)
                {
                    if (!string.IsNullOrEmpty(Database))
                    {
                        Client.PushDatabase(Database);
                    }

                    Client.WriteRecords
                        (
                            _records.ToArray(),
                            false,
                            Actualize
                        );

                    _OnBatchWrite();

                    if (!string.IsNullOrEmpty(Database))
                    {
                        Client.PopDatabase();
                    }
                }
                _records.Clear();
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Flush();
        }

        #endregion
    }
}

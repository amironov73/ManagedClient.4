/* RecordBuffer.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Накапливает записи и отправляет их на сервер пакетами.
    /// </summary>
    public sealed class RecordBuffer
        : IDisposable
    {
	#region Events

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
                lock ( _syncRoot )
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
        /// Initializes a new instance of the <see cref="RecordBuffer" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public RecordBuffer 
            (
                ManagedClient64 client
            )
            : this ( client, 10 )
        {
        }

        public RecordBuffer 
            ( 
                ManagedClient64 client,
                int capacity 
            )
        {
            _syncRoot = new object ();
            _records = new List < IrbisRecord > ();

            if ( capacity <= 0 )
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            Capacity = capacity;

            if ( client == null )
            {
                throw new ArgumentNullException("client");
            }
            Client = client;
        }

        #endregion

        #region Private members

        private readonly object _syncRoot;

        private readonly List < IrbisRecord > _records;

        private string _database;

	private void _OnBatchWrite ()
	{
		EventHandler handler = BatchWrite;
		if (handler != null)
		{
			handler ( this, EventArgs.Empty );
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
                IrbisRecord record
            )
        {
            if ( record == null )
            {
                throw new ArgumentNullException("record");
            }

            lock ( _syncRoot )
            {
                if ( _records.Count >= Capacity )
                {
                    Flush ();
                }
                _records.Add ( record );
                if (_records.Count >= Capacity)
                {
                    Flush();
                }
            }
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush ( )
        {
            lock ( _syncRoot )
            {
                if ( _records.Count != 0 )
                {
                    if ( !string.IsNullOrEmpty ( Database ) )
                    {
                        Client.PushDatabase ( Database );
                    }

                    Client.WriteRecords 
                        ( 
                            _records.ToArray (),
                            false,
                            Actualize
                        );

                    _OnBatchWrite ();

                    if ( !string.IsNullOrEmpty ( Database ) )
                    {
                        Client.PopDatabase ();
                    }
                }
                _records.Clear ();
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ( )
        {
            Flush ();
        }

        #endregion
    }
}

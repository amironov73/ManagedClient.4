// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordProcessor.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Processing
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordProcessor
    {
        #region Events

        /// <summary>
        /// Raised on exception.
        /// </summary>
        public event EventHandler<ExceptionEventArgs<Exception>> ExceptionHandler;

        /// <summary>
        /// Handles the records.
        /// </summary>
        public event RecordHandler RecordHandler;

        #endregion

        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public ProcessingContext Context
        {
            get { return _context; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordProcessor
            (
                [NotNull] ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            _context = new ProcessingContext(client);
        }

        #endregion

        #region Private members

        private readonly ProcessingContext _context;

        #endregion

        #region Public methods

        /// <summary>
        /// Clear accumulated text.
        /// </summary>
        public void Clear()
        {
            Context.Accumulated.Length = 0;
        }

        /// <summary>
        /// Process one record.
        /// </summary>
        public bool ProcessRecord
            (
                [NotNull] IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            Context.Record = record;
            try
            {
                Context.Record = record;
                RecordHandler recordHandler = RecordHandler;
                if (ReferenceEquals(recordHandler, null))
                {
                    throw new Exception();
                }
                ProcessingResult result = recordHandler(Context);
                if (!string.IsNullOrEmpty(result.Result))
                {
                    Context.Accumulated.Append(result.Result);
                }
                if (result.Cancel)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionEventArgs<Exception> eventArgs
                    = new ExceptionEventArgs<Exception>(ex);
                EventHandler<ExceptionEventArgs<Exception>> handler
                    = ExceptionHandler;
                if (!ReferenceEquals(handler, null))
                {
                    handler(this, eventArgs);
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// Process the sequence of records.
        /// </summary>
        public bool ProcessRecords
            (
                [NotNull] IEnumerable<IrbisRecord> records
            )
        {
            if (ReferenceEquals(records, null))
            {
                throw new ArgumentNullException("records");
            }

            foreach (IrbisRecord record in records)
            {
                if (!ProcessRecord(record))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Process the sequence of records.
        /// </summary>
        public bool ProcessRecords
            (
                [NotNull] IEnumerable<int> range
            )
        {
            if (ReferenceEquals(range, null))
            {
                throw new ArgumentNullException("range");
            }

            BatchRecordReader batch = new BatchRecordReader
                (
                    Context.Client,
                    range
                );

            return ProcessRecords(batch);
        }

        /// <summary>
        /// Process all the records in the server database.
        /// </summary>
        public bool ProcessRecords()
        {
            BatchRecordReader batch = new BatchRecordReader
                (
                    Context.Client
                );

            return ProcessRecords(batch);
        }

        /// <summary>
        /// Setup protocol stream.
        /// </summary>
        [NotNull]
        public TextWriter SetProtocol
            (
                [NotNull] TextWriter newProtocol
            )
        {
            if (ReferenceEquals(newProtocol, null))
            {
                throw new ArgumentNullException("newProtocol");
            }

            TextWriter oldStream = Context._protocol;
            Context._protocol = newProtocol;

            return oldStream;
        }

        #endregion
    }
}

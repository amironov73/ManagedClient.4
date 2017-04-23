// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordProcessor.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Processing
{
    public sealed class RecordProcessor
    {
        #region Events

        public event EventHandler<ExceptionEventArgs<Exception>> ExceptionHandler;

        public event RecordHandler RecordHandler;

        #endregion

        #region Properties

        public ProcessingContext Context
        {
            get { return _context; }
        }

        #endregion

        #region Construction

        public RecordProcessor
            (
                ManagedClient64 client
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

        public void Clear()
        {
            Context.Accumulated.Length = 0;
        }

        public bool ProcessRecord
            (
                IrbisRecord record
            )
        {
            Context.Record = record;
            try
            {
                Context.Record = record;
                ProcessingResult result = RecordHandler(Context);
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
                EventHandler<ExceptionEventArgs<Exception>> handler = ExceptionHandler;
                if (handler != null)
                {
                    handler(this, eventArgs);
                }
                return false;
            }
            return true;
        }

        public bool ProcessRecords
            (
                IEnumerable<IrbisRecord> records
            )
        {
            foreach (IrbisRecord record in records)
            {
                if (!ProcessRecord(record))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ProcessRecords
            (
                IEnumerable<int> range
            )
        {
            BatchRecordReader batch = new BatchRecordReader
                (
                    Context.Client,
                    range
                );
            return ProcessRecords(batch);
        }

        public bool ProcessRecords ()
        {
            BatchRecordReader batch = new BatchRecordReader
                (
                    Context.Client
                );
            return ProcessRecords(batch);
        }

        public int[] SearchRecords
            (
                string dictionary,
                string sequential
            )
        {
            return new int[0];
        }

        public TextWriter SetProtocol
            (
                TextWriter newProtocol
            )
        {
            TextWriter oldStream = Context._protocol;
            Context._protocol = newProtocol;
            return oldStream;
        }

        #endregion
    }
}

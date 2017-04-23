﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecord32.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ManagedClient;

#endregion

namespace ManagedClient.Direct
{
    [Serializable]
    [DebuggerDisplay("Leader={Leader}")]
    public sealed class MstRecord32
    {
        #region Constants

        //public const int MstBlockSize = 2048;
        public const int MstBlockSize = 512;

        #endregion

        #region Properties

        public MstRecordLeader32 Leader { get; set; }

        public List<MstDictionaryEntry32> Dictionary { get; set; }

        public bool Deleted
        {
            get { return ((Leader.Status & 
                    (int)
                    (
                        RecordStatus.LogicallyDeleted 
                        | RecordStatus.PhysicallyDeleted)) != 0
                    ); 
                }
        }

        #endregion

        #region Private members

        private string _DumpDictionary()
        {
            StringBuilder result = new StringBuilder();

            foreach (MstDictionaryEntry32 entry in Dictionary)
            {
                result.AppendLine(entry.ToString());
            }

            return result.ToString();
        }

        #endregion

        #region Public methods

        public RecordField DecodeField
            (
                MstDictionaryEntry32 entry
            )
        {
            string catenated = string.Format
                (
                    "{0}#{1}",
                    entry.Tag,
                    entry.Text
                );

            RecordField result = RecordField.Parse(catenated);

            return result;
        }

        public IrbisRecord DecodeRecord()
        {
            IrbisRecord result = new IrbisRecord
            {
                Mfn = Leader.Mfn,
                Status = (RecordStatus)Leader.Status,
                //PreviousOffset = Leader.Previous,
                //Version = Leader.Version
            };

            foreach (MstDictionaryEntry32 entry in Dictionary)
            {
                RecordField field = DecodeField(entry);
                result.Fields.Add(field);
            }

            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "Leader: {0}\r\nDictionary: {1}",
                    Leader,
                    _DumpDictionary()
                );
        }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecord64.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// Record of MST file in IRBIS64.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Leader={Leader}")]
    public sealed class MstRecord64
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Leader.
        /// </summary>
        [CanBeNull]
        public MstRecordLeader64 Leader { get; set; }

        /// <summary>
        /// Dictionary.
        /// </summary>
        [CanBeNull]
        public List<MstDictionaryEntry64> Dictionary { get; set; }

        /// <summary>
        /// Record deleted?
        /// </summary>
        public bool Deleted
        {
            get
            {
                return (Leader.Status & (int)
                    (RecordStatus.LogicallyDeleted 
                    | RecordStatus.PhysicallyDeleted)) != 0;
            }
        }

        #endregion

        #region Private members

        private string _DumpDictionary ( )
        {
            StringBuilder result = new StringBuilder();

            foreach ( MstDictionaryEntry64 entry in Dictionary )
            {
                result.AppendLine ( entry.ToString () );
            }

            return result.ToString ();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Decode the field.
        /// </summary>
        [NotNull]
        public RecordField DecodeField
            (
                [NotNull] MstDictionaryEntry64 entry
            )
        {
            string catenated = string.Concat
                (
                    entry.Tag,
                    "#",
                    entry.Text
                );

            RecordField result = RecordField.Parse(catenated);

            return result;
        }

        /// <summary>
        /// Decode entire the record.
        /// </summary>
        [NotNull]
        public IrbisRecord DecodeRecord()
        {
            IrbisRecord result = new IrbisRecord
                {
                    Mfn = Leader.Mfn,
                    Status = (RecordStatus) Leader.Status,
                    PreviousOffset = Leader.Previous,
                    Version = Leader.Version
                };

            foreach (MstDictionaryEntry64 entry in Dictionary)
            {
                RecordField field = DecodeField(entry);
                result.Fields.Add(field);
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "Leader: {0}\r\nDictionary: {1}", 
                    Leader,
                    _DumpDictionary ()
                );
        }

        #endregion
    }
}

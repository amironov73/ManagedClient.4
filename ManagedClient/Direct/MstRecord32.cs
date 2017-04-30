// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecord32.cs
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
    /// Record of MST file in IRBIS32.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Leader={Leader}")]
    public sealed class MstRecord32
    {
        #region Constants

        /// <summary>
        /// Size of MST file block.
        /// </summary>
        public const int MstBlockSize = 512;

        #endregion

        #region Properties

        /// <summary>
        /// Record leader.
        /// </summary>
        [NotNull]
        // ReSharper disable NotNullMemberIsNotInitialized
        public MstRecordLeader32 Leader { get; set; }
        // ReSharper restore NotNullMemberIsNotInitialized

        /// <summary>
        /// Dictionary of the fields.
        /// </summary>
        public List<MstDictionaryEntry32> Dictionary { get; set; }

        /// <summary>
        /// Whether the record is deleted?
        /// </summary>
        public bool Deleted
        {
            get
            {
                return 
                    (
                        Leader.Status & 
                        (int)
                        (
                            RecordStatus.LogicallyDeleted 
                            | RecordStatus.PhysicallyDeleted)
                        ) != 0; 
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

        /// <summary>
        /// Decode the field.
        /// </summary>
        public RecordField DecodeField
            (
                [NotNull] MstDictionaryEntry32 entry
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

        /// <summary>
        /// Decode entire the record.
        /// </summary>
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

        /// <inheritdoc cref="object.ToString"/>
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

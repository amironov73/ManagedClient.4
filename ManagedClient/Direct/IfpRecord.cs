// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IfpRecord.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// IFP file record.
    /// </summary>
    [Serializable]
    public sealed class IfpRecord
    {
        #region Constants

        /// <summary>
        /// Число ссылок на термин, после превышения которого
        /// используется специальный блок ссылок.
        /// </summary>
        public const int MinPostingsInBlock = 256;

        #endregion

        #region Properties

        /// <summary>
        /// Low part of the offset.
        /// </summary>
        public int LowOffset { get; set; }

        /// <summary>
        /// High part of the offset.
        /// </summary>
        public int HighOffset { get; set; }

        /// <summary>
        /// Total link count.
        /// </summary>
        public int TotalLinkCount { get; set; }

        /// <summary>
        /// Block link count.
        /// </summary>
        public int BlockLinkCount { get; set; }

        /// <summary>
        /// Capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Links.
        /// </summary>
        [NotNull]
        public List<TermLink> Links { get { return _links; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<TermLink> _links = new List<TermLink>();

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record.
        /// </summary>
        public static IfpRecord Read
            (
                [NotNull] Stream stream,
                long offset
            )
        {
            //new ObjectDumper()
            //    .DumpStream(stream, offset, 100);

            stream.Position = offset;

            IfpRecord result = new IfpRecord
                {
                    LowOffset = stream.ReadInt32Network(),
                    HighOffset = stream.ReadInt32Network(),
                    TotalLinkCount = stream.ReadInt32Network(),
                    BlockLinkCount = stream.ReadInt32Network(),
                    Capacity = stream.ReadInt32Network()
                };

            for (int i = 0; i < result.BlockLinkCount; i++)
            {
                TermLink link = TermLink.Read(stream);
                result.Links.Add(link);
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (TermLink link in Links)
            {
                builder.AppendLine(link.ToString());
            }

            return string.Format
                (
                    "LowOffset: {0}, HighOffset: {1}, TotalLinkCount: {2}, "
                    + "BlockLinkCount: {3}, Capacity: {4}\r\nItems: {5}", 
                    LowOffset, 
                    HighOffset,
                    TotalLinkCount,
                    BlockLinkCount,
                    Capacity,
                    builder
                );
        }

        #endregion
    }
}

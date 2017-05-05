// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeRecord.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Node record.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Leader={Leader}")]
    public sealed class NodeRecord
    {
        #region Constants

        /// <summary>
        /// Длина записи в текущей реализации.
        /// </summary>
        public const int RecordSize = 2048;

        #endregion

        #region Properties

        public bool IsLeaf { get; private set; }

        public NodeLeader Leader { get; set; }

        public List<NodeItem> Items { get { return _items; } }

        #endregion

        #region Construction

        public NodeRecord()
        {
            Leader = new NodeLeader();
            _items = new List<NodeItem>();
        }

        public NodeRecord(bool isLeaf)
            : this()
        {
            IsLeaf = isLeaf;
        }

        #endregion

        #region Private members

        private readonly List<NodeItem> _items;

        internal Stream _stream;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder items = new StringBuilder();
            foreach (NodeItem item in Items)
            {
                items.AppendLine(item.ToString());
            }

            return string.Format
                (
                    "Leader: {0}, Items: {1}",
                    Leader,
                    items
                );
        }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeLeader.cs
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Лидер записи в N01, L01
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Number={Number}, Previous={Previous}, Next={Next}, "
        + "TermCount={TermCount}, FreeOffset={FreeOffset}")]
    public sealed class NodeLeader
    {
        #region Properties

        /// <summary>
        /// Номер записи (начиная с 1; в N01 номер первой записи
        /// равен номеру корневой записи дерева
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Номер предыдущей записи (-1, если нет)
        /// </summary>
        public int Previous { get; set; }

        /// <summary>
        /// Номер следующей записи (-1, если нет)
        /// </summary>
        public int Next { get; set; }

        /// <summary>
        /// Число ключей в записи
        /// </summary>
        public int TermCount { get; set; }

        /// <summary>
        /// Смещение на свободную позицию в записи
        /// (от начала записи)
        /// </summary>
        public int FreeOffset { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the node from the stream.
        /// </summary>
        [NotNull]
        public static NodeLeader Read
            (
                [NotNull] Stream stream
            )
        {
            NodeLeader result = new NodeLeader
                {
                    Number = stream.ReadInt32Network(),
                    Previous = stream.ReadInt32Network(),
                    Next = stream.ReadInt32Network(),
                    TermCount = stream.ReadInt32Network(),
                    FreeOffset = stream.ReadInt32Network()
                };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return string.Format
                (
                    "Number: {0}, Previous: {1}, "
                    + "Next: {2}, TermCount: {3}, "
                    + "FreeOffset: {4}", 
                    Number, 
                    Previous, 
                    Next, 
                    TermCount, 
                    FreeOffset
                );
        }

        #endregion
    }
}

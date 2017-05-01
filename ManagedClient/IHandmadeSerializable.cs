// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IHandmadeSerializable.cs -- объект умеет сохраняться в поток
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Объект умеет сохраняться в поток и восстанавливаться из него.
    /// </summary>
    public interface IHandmadeSerializable
    {
        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        void SaveToStream
            (
                [NotNull] BinaryWriter writer
            );

        /// <summary>
        /// Просим объект восстановить себя из потока.
        /// </summary>
        void ReadFromStream
            (
                [NotNull] BinaryReader reader
            );
    }
}

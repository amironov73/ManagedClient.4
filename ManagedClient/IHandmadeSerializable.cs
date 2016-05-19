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
    }
}

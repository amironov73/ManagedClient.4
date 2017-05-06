// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisWorkstation.cs
 */

namespace ManagedClient
{
    /// <summary>
    /// Коды АРМов ИРБИС.
    /// </summary>
    public enum IrbisWorkstation
        : byte
    {
        /// <summary>
        /// Не задан.
        /// </summary>
        None = 0,

        /// <summary>
        /// АРМ "Администратор"
        /// </summary>
        Administrator = (byte) 'A',

        /// <summary>
        /// АРМ "Каталогизатор"
        /// </summary>
        Cataloger = (byte)'C',

        /// <summary>
        /// АРМ "Комплектатор"
        /// </summary>
        Acquisitions = (byte)'M',

        /// <summary>
        /// АРМ "Читатель"
        /// </summary>
        Reader = (byte) 'R',

        /// <summary>
        /// АРМ "Книговыдача"
        /// </summary>
        Circulation = (byte) 'B',

        /// <summary>
        /// АРМ "Книговыдача"
        /// </summary>
        Bookland = (byte) 'B',

        /// <summary>
        /// АРМ "Книгообеспеченность"
        /// </summary>
        Provision = (byte) 'K'

    }
}

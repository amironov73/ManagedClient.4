// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SelectionType.cs -- тип отбора записей.
 */

namespace ManagedClient.SubBasing
{
    /// <summary>
    /// Тип отбора записей
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// По MFN
        /// </summary>
        Mfn,

        /// <summary>
        /// По возрастающим числам
        /// (например, инвентарным номерам).
        /// </summary>
        Sequential,

        /// <summary>
        /// Элементы словаря.
        /// </summary>
        Dictionary,

        /// <summary>
        /// Согласно поисковому выражению.
        /// </summary>
        Search,

        /// <summary>
        /// Глубокий последовательный поиск.
        /// </summary>
        Deep,

        /// <summary>
        /// Отбор скриптом
        /// </summary>
        Script
    }
}

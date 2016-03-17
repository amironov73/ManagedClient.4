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

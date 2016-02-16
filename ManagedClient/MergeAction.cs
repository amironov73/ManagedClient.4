/* MergeAction.cs
 */

#region Using directives

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Действия над полями при слиянии записей.
    /// </summary>
    public enum MergeAction
    {
        /// <summary>
        /// Не заменять, оставлять прежним.
        /// </summary>
        NoAction = 0,

        /// <summary>
        /// Не заменять, оставлять прежним.
        /// </summary>
        LeaveIntact = 0,

        /// <summary>
        /// Полностью заменять.
        /// </summary>
        Replace = 1,

        /// <summary>
        /// Добавлять все повторения.
        /// </summary>
        AppendAll = 2,

        /// <summary>
        /// Добавлять только отсутствующие повторения.
        /// </summary>
        AppendNew = 3,

        /// <summary>
        /// Добавлять, только если нет такого поля.
        /// </summary>
        AppendIfEmpty = 4
    }
}

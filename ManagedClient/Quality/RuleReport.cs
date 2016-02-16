/* RuleReport.cs -- отчёт о работе правила.
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Отчёт о работе правила.
    /// </summary>
    [Serializable]
    public sealed class RuleReport
    {
        #region Properties

        /// <summary>
        /// Дефекты, обнаруженные правилом.
        /// </summary>
        public List<FieldDefect> Defects { get; set; }

        /// <summary>
        /// Общий урон.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Начисленный бонус.
        /// </summary>
        public int Bonus { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RuleReport()
        {
            Defects = new List<FieldDefect>();
        }

        #endregion
    }
}

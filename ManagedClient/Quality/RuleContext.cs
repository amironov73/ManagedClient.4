/* RuleContext.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Quality
{
    [Serializable]
    public sealed class RuleContext
    {
        #region Properties

        /// <summary>
        /// Клиент.
        /// </summary>
        public ManagedClient64 Client { get; set; }

        /// <summary>
        /// Обрабатываемая запись.
        /// </summary>
        public IrbisRecord Record { get; set; }

        /// <summary>
        /// Формат для краткого библиографического описания.
        /// </summary>
        public string BriefFormat { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RuleContext()
        {
            BriefFormat = "@brief";
        }

        #endregion
    }
}

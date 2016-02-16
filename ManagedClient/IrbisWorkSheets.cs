/* IrbisWorksheets.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Некоторые хорошо известные рабочие листы.
    /// </summary>
    public sealed class IrbisWorkSheets
    {
        #region Constants

        /// <summary>
        /// Выпуск (номер) журнала.
        /// </summary>
        public const string MagazineIssue = "NJ";

        /// <summary>
        /// Сводное описание журнала.
        /// </summary>
        public const string Magazine = "J";

        /// <summary>
        /// Статья из журнала.
        /// </summary>
        public const string MagazineArticle = "ASP";

        #endregion
    }
}

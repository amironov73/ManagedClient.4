/* IrbisAnswerItem.cs
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
    /// Элемент ответа сервера на поисковый запрос.
    /// </summary>
    [Serializable]
    public sealed class IrbisAnswerItem
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// MFN записи.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Собственно запись.
        /// </summary>
        public IrbisRecord Record { get; set; }

        /// <summary>
        /// Расформатированная запись.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Ключ (для сортировки).
        /// </summary>
        public string Key { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}

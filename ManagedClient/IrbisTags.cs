/* IrbisTags.cs
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
    /// Стандартные теги ИРБИС.
    /// </summary>
    public static class IrbisTags
    {
        #region Constants

        /// <summary>
        /// Основное заглавие.
        /// </summary>
        public const string MainTitle = "200";

        /// <summary>
        /// Шифр документа в базе.
        /// </summary>
        public const string DocumentCode = "903";
        
        /// <summary>
        /// Шифр журнала.
        /// </summary>
        public const string MagazineCode = "933";

        /// <summary>
        /// Год выпуска журнала.
        /// </summary>
        public const string MagazineYear = "934";

        #endregion

    }
}

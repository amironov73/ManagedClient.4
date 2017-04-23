// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineArticleInfo.cs
 */

#region Using directives

using System;
using System.Xml.Serialization;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Magazines
{
    /// <summary>
    /// Информация о статье.
    /// </summary>
    [Serializable]
    [XmlRoot("article")]
    [MoonSharpUserData]
    public sealed class MagazineArticleInfo
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineArticleInfo Parse
            (
                IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            MagazineArticleInfo result = new MagazineArticleInfo();
            return result;
        }

        /// <summary>
        /// Разбор поля (330 или 922).
        /// </summary>
        public static MagazineArticleInfo Parse
            (
                RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                throw new ArgumentNullException("field");
            }

            MagazineArticleInfo result = new MagazineArticleInfo();
            return result;
        }

        #endregion

        #region Object members



        #endregion
    }
}

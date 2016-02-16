/* MagazineInfo.cs
 */

#region Using directives

using System;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Magazines
{
    /// <summary>
    /// Информация о журнале в целом.
    /// </summary>
    [Serializable]
    public sealed class MagazineInfo
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Код документа в базе. Поле 903.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Заглавие. Поле 200^a
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Подзаголовочные сведения.
        /// Поле 200^e.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Обозначение и выпуск серии.
        /// Поле 923^1.
        /// </summary>
        public string SeriesNumber { get; set; }

        /// <summary>
        /// Заголовок серии.
        /// Поле 923^i.
        /// </summary>
        public string SeriesTitle { get; set; }

        /// <summary>
        /// Расширенное заглавие. 
        /// Включает заголовок выпуск и заголовок серии.
        /// </summary>
        public string ExtendedTitle
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(Title);
                if (!string.IsNullOrEmpty(SeriesNumber))
                {
                    result.AppendFormat(". {0}", SeriesNumber);
                }
                if (!string.IsNullOrEmpty(SeriesTitle))
                {
                    result.AppendFormat(". {0}", SeriesTitle);
                }
                if (!string.IsNullOrEmpty(SubTitle))
                {
                    result.AppendFormat(": {0}", SubTitle);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Тип издания. Поле 110^t
        /// </summary>
        public string MagazineType { get; set; }

        /// <summary>
        /// Вид издания. Поле 110^b
        /// </summary>
        public string MagazineKind { get; set; }

        /// <summary>
        /// Периодичность (число). Поле 110^x
        /// </summary>
        public string Periodicity { get; set; }

        /// <summary>
        /// Кумуляция. Поле 909
        /// </summary>
        public MagazineCumulation[] Cumulation { get; set; }

        /// <summary>
        /// MFN записи журнала.
        /// </summary>
        public int Mfn { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">record</exception>
        public static MagazineInfo Parse
            (
                IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            MagazineInfo result = new MagazineInfo
            {
                Index = record.FM("903"),
                Title = record.FM("200", 'a'),
                SubTitle = record.FM("200", 'e'),
                Cumulation = record.Fields
                    .GetField("909")
                    .Select(field => MagazineCumulation.Parse(field))
                    .ToArray(),
                SeriesNumber = record.FM("923",'h'),
                SeriesTitle = record.FM("923", 'i'),
                Mfn = record.Mfn
            };

            if (string.IsNullOrEmpty(result.Title)
                || string.IsNullOrEmpty(result.Index)
                //|| string.IsNullOrEmpty(result.MagazineKind)
                //|| string.IsNullOrEmpty(result.MagazineType)
                )
            {
                return null;
            }

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ExtendedTitle;
        }

        #endregion
    }
}

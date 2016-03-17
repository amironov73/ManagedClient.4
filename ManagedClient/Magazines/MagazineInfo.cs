/* MagazineInfo.cs
 */

#region Using directives

using System;
using System.Text;
using System.Xml.Serialization;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Magazines
{
    /// <summary>
    /// Информация о журнале в целом.
    /// </summary>
    [Serializable]
    [XmlRoot("magazine")]
    [MoonSharpUserData]
    public sealed class MagazineInfo
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Код документа в базе. Поле 903.
        /// </summary>
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Заглавие. Поле 200^a
        /// </summary>
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Подзаголовочные сведения.
        /// Поле 200^e.
        /// </summary>
        [XmlAttribute("sub-title")]
        [JsonProperty("sub-title")]
        public string SubTitle { get; set; }

        /// <summary>
        /// Обозначение и выпуск серии.
        /// Поле 923^1.
        /// </summary>
        [XmlAttribute("series-number")]
        [JsonProperty("series-number")]
        public string SeriesNumber { get; set; }

        /// <summary>
        /// Заголовок серии.
        /// Поле 923^i.
        /// </summary>
        [XmlAttribute("series-title")]
        [JsonProperty("series-title")]
        public string SeriesTitle { get; set; }

        /// <summary>
        /// Расширенное заглавие. 
        /// Включает заголовок выпуск и заголовок серии.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
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
        [XmlAttribute("magazine-type")]
        [JsonProperty("magazine-type")]
        public string MagazineType { get; set; }

        /// <summary>
        /// Вид издания. Поле 110^b
        /// </summary>
        [XmlAttribute("magazine-kind")]
        [JsonProperty("magazine-kind")]
        public string MagazineKind { get; set; }

        /// <summary>
        /// Периодичность (число). Поле 110^x
        /// </summary>
        public string Periodicity { get; set; }

        /// <summary>
        /// Кумуляция. Поле 909
        /// </summary>
        [XmlElement("cumulation")]
        [JsonProperty("cumulation")]
        public MagazineCumulation[] Cumulation { get; set; }

        /// <summary>
        /// MFN записи журнала.
        /// </summary>
        [XmlElement("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
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
                Cumulation = MagazineCumulation.Parse(record),
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

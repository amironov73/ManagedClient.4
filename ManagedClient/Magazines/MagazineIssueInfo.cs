/* MagazineIssueInfo.cs -- сведения о номере журнала
 */

#region Using directives

using System;
using System.Linq;
using System.Xml.Serialization;

using ManagedClient.Fields;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Magazines
{
    /// <summary>
    /// Сведения о номере журнала
    /// </summary>
    [Serializable]
    [XmlRoot("issue")]
    [MoonSharpUserData]
    public sealed class MagazineIssueInfo
    {
        #region Properties

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Шифр документа в базе. Поле 903.
        /// </summary>
        [XmlAttribute("document-code")]
        [JsonProperty("document-code")]
        public string DocumentCode { get; set; }

        /// <summary>
        /// Шифр журнала. Поле 933.
        /// </summary>
        [XmlAttribute("magazine-code")]
        [JsonProperty("magazine-code")]
        public string MagazineCode { get; set; }

        /// <summary>
        /// Год. Поле 934.
        /// </summary>
        [XmlAttribute("year")]
        [JsonProperty("year")]
        public string Year { get; set; }

        /// <summary>
        /// Том. Поле 935.
        /// </summary>
        [XmlAttribute("volume")]
        [JsonProperty("volume")]
        public string Volume { get; set; }

        /// <summary>
        /// Номер, часть. Поле 936.
        /// </summary>
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// Дополнение к номеру. Поле 931^c.
        /// </summary>
        [XmlAttribute("supplement")]
        [JsonProperty("supplement")]
        public string Supplement { get; set; }

        /// <summary>
        /// Рабочий лист. Поле 920.
        /// (чтобы отличать подшивки от выпусков журналов)
        /// </summary>
        [XmlAttribute("worksheet")]
        [JsonProperty("worksheet")]
        public string Worksheet { get; set; }

        /// <summary>
        /// Расписанное оглавление. Поле 922.
        /// </summary>
        [XmlElement("article")]
        [JsonProperty("articles")]
        public MagazineArticleInfo[] Articles { get; set; }

        /// <summary>
        /// Экземпляры. Поле 910.
        /// </summary>
        [XmlElement("exemplar")]
        [JsonProperty("exemplars")]
        public ExemplarInfo[] Exemplars { get; set; }

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
        public static MagazineIssueInfo Parse
            (
                IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            MagazineIssueInfo result = new MagazineIssueInfo
            {
                Mfn = record.Mfn,
                DocumentCode = record.FM("903"),
                MagazineCode = record.FM("933"),
                Year = record.FM("934"),
                Volume = record.FM("935"),
                Number = record.FM("936"),
                Supplement = record.FM("931", 'c'),
                Worksheet = record.FM("920"),
                Articles = record.Fields
                    .GetField("922")
                    .Select(MagazineArticleInfo.Parse)
                    .ToArray(),
                Exemplars = record.Fields
                    .GetField("910")
                    .Select(ExemplarInfo.Parse)
                    .ToArray()
            };

            if (string.IsNullOrEmpty(result.Number))
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Сравнение двух выпусков
        /// (с целью сортировки по возрастанию номеров).
        /// </summary>
        public static int CompareNumbers
            (
                MagazineIssueInfo first,
                MagazineIssueInfo second
            )
        {
            return NumberText.Compare(first.Number, second.Number);
        }

        #endregion

        #region Object info

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Supplement))
            {
                return string
                    .Format("{0} ({1})", Number, Supplement)
                    .Trim();
            }
            return Number.Trim();
        }

        #endregion
    }
}

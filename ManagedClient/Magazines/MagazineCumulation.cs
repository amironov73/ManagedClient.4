/* MagazineCumulation.cs -- данные о кумуляции номеров
 */

#region Using directives

using System;
using System.Linq;
using System.Xml.Serialization;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Magazines
{
    /// <summary>
    /// Данные о кумуляции номеров. Поле 909.
    /// </summary>
    [Serializable]
    [XmlRoot("cumulation")]
    [MoonSharpUserData]
    public sealed class MagazineCumulation
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "909";

        #endregion

        #region Properties

        /// <summary>
        /// Год. Подполе Q.
        /// </summary>
        [XmlAttribute("year")]
        [JsonProperty("year")]
        public string Year { get; set; }

        /// <summary>
        /// Том. Подполе F.
        /// </summary>
        [XmlAttribute("volume")]
        [JsonProperty("volume")]
        public string Volume { get; set; }

        /// <summary>
        /// Место хранения. Подполе D.
        /// </summary>
        [XmlAttribute("place")]
        [JsonProperty("place")]
        public string Place { get; set; }

        /// <summary>
        /// Кумулированные номера. Подполе H.
        /// </summary>
        [XmlAttribute("numbers")]
        [JsonProperty("numbers")]
        public string Numbers { get; set; }

        /// <summary>
        /// Номер комплекта. Подполе K.
        /// </summary>
        [XmlAttribute("complect")]
        [JsonProperty("complect")]
        public string Complect { get; set; }

        #endregion

        #region Construciton

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static MagazineCumulation Parse
            (
                RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                throw new ArgumentNullException("field");
            }

            MagazineCumulation result = new MagazineCumulation
            {
                Year = field.GetFirstSubFieldText('q'),
                Volume = field.GetFirstSubFieldText('f'),
                Place = field.GetFirstSubFieldText('d'),
                Numbers = field.GetFirstSubFieldText('h'),
                Complect = field.GetFirstSubFieldText('k')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineCumulation[] Parse
            (
                IrbisRecord record,
                string tag
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }

            return record.Fields
                .GetField(tag)
                .Select(Parse)
                .ToArray();
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineCumulation[] Parse
            (
                IrbisRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        #endregion

        #region Object members

        #endregion
    }
}

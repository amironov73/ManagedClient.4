/* RevisionInfo.cs -- данные о редактировании записи
 */

#region Using directives

using System;
using System.Linq;
using System.Xml.Serialization;

using ManagedClient.Mapping;

using MoonSharp.Interpreter;
using Newtonsoft.Json;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Данные о редактировании записи (поле 907).
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    public sealed class RevisionInfo
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "907";

        #endregion

        #region Properties

        /// <summary>
        /// Этап работы. Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("stage")]
        [JsonProperty("stage")]
        public string Stage { get; set; }

        /// <summary>
        /// Дата. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// ФИО оператора. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static RevisionInfo Parse
            (
                RecordField field
            )
        {
            RevisionInfo result = new RevisionInfo
                {
                    Date = field.GetSubFieldText('a', 0),
                    Name = field.GetSubFieldText('b', 0),
                    Stage = field.GetSubFieldText('c', 0)
                };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static RevisionInfo[] Parse
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
        public static RevisionInfo[] Parse
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

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        public RecordField ToField()
        {
            RecordField result = new RecordField("907")
                .AddNonEmptySubField('a', Date)
                .AddNonEmptySubField('b', Name)
                .AddNonEmptySubField('c', Stage);
            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Stage: {0}, Date: {1}, Name: {2}",
                    Stage,
                    Date,
                    Name
                );
        }

        #endregion
    }
}

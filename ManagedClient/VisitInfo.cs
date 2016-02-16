/* VisitInfo.cs -- информация о посещении/выдаче.
 */

#region Using directives

using System;
using System.Text;
using System.Xml.Serialization;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Информация о посещении/выдаче.
    /// </summary>
    [Serializable]
    [XmlRoot("visit")]
    public sealed class VisitInfo
    {
        #region Properties

        /// <summary>
        /// подполе G, имя БД каталога.
        /// </summary>
        [XmlAttribute("database")]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// подполе A, шифр документа.
        /// </summary>
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// подполе B, инвентарный номер экземпляра
        /// </summary>
        [XmlAttribute("inventory")]
        [JsonProperty("inventory")]
        public string Inventory { get; set; }

        /// <summary>
        /// подполе H, штрих-код экземпляра.
        /// </summary>
        [XmlAttribute("barcode")]
        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// подполе K, место хранения экземпляра
        /// </summary>
        [XmlAttribute("sigla")]
        [JsonProperty("sigla")]
        public string Sigla { get; set; }

        /// <summary>
        /// подполе D, дата выдачи
        /// </summary>
        [XmlAttribute("date-given")]
        [JsonProperty("date-given")]
        public string DateGivenString { get; set; }

        /// <summary>
        /// подполе V, место выдачи
        /// </summary>
        [XmlAttribute("department")]
        [JsonProperty("department")]
        public string Department { get; set; }

        /// <summary>
        /// подполе E, дата предполагаемого возврата
        /// </summary>
        [XmlAttribute("date-expected")]
        [JsonProperty("date-expected")]
        public string DateExpectedString { get; set; }

        /// <summary>
        /// подполе F, дата фактического возврата
        /// </summary>
        [XmlAttribute("date-returned")]
        [JsonProperty("date-returned")]
        public string DateReturnedString { get; set; }

        /// <summary>
        /// подполе L, дата продления
        /// </summary>
        [XmlAttribute("date-prolong")]
        [JsonProperty("date-prolong")]
        public string DateProlongString { get; set; }

        /// <summary>
        /// подполе U, признак утерянной книги
        /// </summary>
        [XmlAttribute("lost")]
        [JsonProperty("lost")]
        public string Lost { get; set; }

        /// <summary>
        /// подполе C, краткое библиографическое описание
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// подполе I, ответственное лицо
        /// </summary>
        [XmlAttribute("responsible")]
        [JsonProperty("responsible")]
        public string Responsible { get; set; }

        /// <summary>
        /// подполе 1, время начала визита в библиотеку
        /// </summary>
        [XmlAttribute("time-in")]
        [JsonProperty("time-in")]
        public string TimeIn { get; set; }

        /// <summary>
        /// подполе 2, время окончания визита в библиотеку
        /// </summary>
        [XmlAttribute("time-out")]
        [JsonProperty("time-out")]
        public string TimeOut { get; set; }

        /// <summary>
        /// Не посещение ли?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsVisit
        {
            get { return string.IsNullOrEmpty(Index); }
        }

        /// <summary>
        /// Возвращена ли книга?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsReturned
        {
            get
            {
                return !string.IsNullOrEmpty(DateReturnedString)
                       && !DateReturnedString.StartsWith("*");
            }
        }

        /// <summary>
        /// Дата выдачи/посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateGiven
        {
            get
            {
                return DateGivenString.ParseIrbisDate();
            }
        }

        /// <summary>
        /// Дата возврата
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateReturned
        {
            get
            {
                return DateReturnedString.ParseIrbisDate();
            }
        }

        /// <summary>
        /// Ожидаемая дата возврата
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateExpected
        {
            get
            {
                return DateExpectedString.ParseIrbisDate();
            }
        }

        #endregion

        #region Private members

        private static string FM
            (
                RecordField field,
                char code
            )
        {
            return field.GetSubFieldText(code, 0);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>VisitInfo.</returns>
        public static VisitInfo Parse
            (
                RecordField field
            )
        {
            VisitInfo result = new VisitInfo
            {
                Database = FM(field, 'g'),
                Index = FM(field, 'a'),
                Inventory = FM(field, 'b'),
                Barcode = FM(field, 'h'),
                Sigla = FM(field, 'k'),
                DateGivenString = FM(field, 'd'),
                Department = FM(field, 'v'),
                DateExpectedString = FM(field, 'e'),
                DateReturnedString = FM(field, 'f'),
                DateProlongString = FM(field, 'l'),
                Lost = FM(field, 'u'),
                Description = FM(field, 'c'),
                Responsible = FM(field, 'i'),
                TimeIn = FM(field, '1'),
                TimeOut = FM(field, '2')
            };

            return result;
        }

        /// <summary>
        /// Формирование поля 40 
        /// из данных о выдаче/посещении.
        /// </summary>
        /// <returns></returns>
        public RecordField ToField()
        {
            //RecordField result = new RecordField("40");
            throw new NotImplementedException();
            //return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .AppendFormat("Посещение: \t\t\t{0}", IsVisit)
                .AppendLine()
                .AppendFormat("Описание: \t\t\t{0}", Description)
                .AppendLine()
                .AppendFormat("Шифр документа: \t\t{0}", Index)
                .AppendLine()
                .AppendFormat("Штрих-код: \t\t\t{0}", Barcode)
                .AppendLine()
                .AppendFormat("Место хранения: \t\t{0}", Sigla)
                .AppendLine()
                .AppendFormat("Дата выдачи: \t\t\t{0:d}", DateGiven)
                .AppendLine()
                .AppendFormat("Место выдачи: \t\t\t{0}", Department)
                .AppendLine()
                .AppendFormat("Ответственное лицо: \t\t{0}", Responsible)
                .AppendLine()
                .AppendFormat("Дата предполагаемого возврата: \t{0:d}", DateExpected)
                .AppendLine()
                .AppendFormat("Возвращена: \t\t\t{0}", IsReturned)
                .AppendLine()
                .AppendFormat("Дата возврата: \t\t\t{0:d}", DateReturned)
                .AppendLine()
                .AppendLine(new string('-', 60));

            return result.ToString();
        }

        #endregion
    }
}

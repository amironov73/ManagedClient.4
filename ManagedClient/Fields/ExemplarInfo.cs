/* ExemplarInfo.cs -- информация об экземпляре (поле 910).
 */

#region Using directives

using System;
using System.Linq;
using System.Xml.Serialization;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

using ManagedClient.Mapping;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Информация об экземпляре (поле 910).
    /// </summary>
    [Serializable]
    [XmlRoot("exemplar")]
    [MoonSharpUserData]
    public sealed class ExemplarInfo
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "!=0124abcdefhiknpqrstuvwxyz";

        /// <summary>
        /// Тег полей, содержащих сведения об экземплярах.
        /// </summary>
        public const string ExemplarTag = "910";

        #endregion

        #region Properties

        /// <summary>
        /// Статус. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Инвентарный номер. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// Дата поступления. Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Место хранения. Подполе d.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("place")]
        [JsonProperty("place")]
        public string Place { get; set; }

        /// <summary>
        /// Наименование коллекции. Подполе q.
        /// </summary>
        [SubField('q')]
        [XmlAttribute("collection")]
        [JsonProperty("collection")]
        public string Collection { get; set; }

        /// <summary>
        /// Расстановочный шифр. Подполе r.
        /// </summary>
        [SubField('r')]
        [XmlAttribute("shelf-index")]
        [JsonProperty("shelf-index")]
        public string ShelfIndex { get; set; }

        /// <summary>
        /// Цена экземпляра. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("price")]
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// Штрих-код/радиометка. Подполе h.
        /// </summary>
        [SubField('h')]
        [XmlAttribute("barcode")]
        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Число экземпляров. Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("amount")]
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Специальное назначение фонда. Подполе t.
        /// </summary>
        [SubField('t')]
        [XmlAttribute("purpose")]
        [JsonProperty("purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// Коэффициент многоразового использования. Подполе =.
        /// </summary>
        [SubField('=')]
        [XmlAttribute("coefficient")]
        [JsonProperty("coefficient")]
        public string Coefficient { get; set; }

        /// <summary>
        /// Экземпляры не на баланс. Подполе 4.
        /// </summary>
        [SubField('4')]
        [XmlAttribute("off-balance")]
        [JsonProperty("off-balance")]
        public string OffBalance { get; set; }

        /// <summary>
        /// Номер записи КСУ. Подполе u.
        /// </summary>
        [SubField('u')]
        [XmlAttribute("ksu-number1")]
        [JsonProperty("ksu-number1")]
        public string KsuNumber1 { get; set; }

        /// <summary>
        /// Номер акта. Подполе y.
        /// </summary>
        [SubField('y')]
        [XmlAttribute("act-number1")]
        [JsonProperty("act-number1")]
        public string ActNumber1 { get; set; }

        /// <summary>
        /// Канал поступления. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("channel")]
        [JsonProperty("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// Число выданных экземпляров. Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("on-hand")]
        [JsonProperty("on-hand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Номер акта списания. Подполе v.
        /// </summary>
        [SubField('v')]
        [XmlAttribute("act-number2")]
        [JsonProperty("act-number2")]
        public string ActNumber2 { get; set; }

        /// <summary>
        /// Количество списываемых экземпляров. Подполе x.
        /// </summary>
        [SubField('x')]
        [XmlAttribute("write-off")]
        [JsonProperty("write-off")]
        public string WriteOff { get; set; }

        /// <summary>
        /// Количество экземпляров для докомплектования. Подполе k.
        /// </summary>
        [SubField('k')]
        [XmlAttribute("completion")]
        [JsonProperty("completion")]
        public string Completion { get; set; }

        /// <summary>
        /// Номер акта передачи в другое подразделение. Подполе w.
        /// </summary>
        [SubField('w')]
        [XmlAttribute("act-number3")]
        [JsonProperty("act-number3")]
        public string ActNumber3 { get; set; }

        /// <summary>
        /// Количество передаваемых экземпляров. Подполе z.
        /// </summary>
        [SubField('z')]
        [XmlAttribute("moving")]
        [JsonProperty("moving")]
        public string Moving { get; set; }

        /// <summary>
        /// Нове место хранения. Подполе m.
        /// </summary>
        [SubField('m')]
        [XmlAttribute("new-place")]
        [JsonProperty("new-place")]
        public string NewPlace { get; set; }

        /// <summary>
        /// Дата проверки фонда. Подполе s.
        /// </summary>
        [SubField('s')]
        [XmlAttribute("checked-date")]
        [JsonProperty("checked-date")]
        public string CheckedDate { get; set; }

        /// <summary>
        /// Число проверенных экземпляров. Подполе 0.
        /// </summary>
        [SubField('0')]
        [XmlAttribute("checked-amount")]
        [JsonProperty("checked-amount")]
        public string CheckedAmount { get; set; }

        /// <summary>
        /// Реальное место нахождения книги. Подполе !.
        /// </summary>
        [SubField('!')]
        [XmlAttribute("real-place")]
        [JsonProperty("real-place")]
        public string RealPlace { get; set; }

        /// <summary>
        /// Шифр подшивки. Подполе p.
        /// </summary>
        [SubField('p')]
        [XmlAttribute("binding-index")]
        [JsonProperty("binding-index")]
        public string BindingIndex { get; set; }

        /// <summary>
        /// Инвентарный номер подшивки. Подполе i.
        /// </summary>
        [SubField('i')]
        [XmlAttribute("binding-number")]
        [JsonProperty("binding-number")]
        public string BindingNumber { get; set; }

        /// <summary>
        /// Прочие подполя, не попавшие в вышеперечисленные.
        /// </summary>
        [XmlElement("other-subfields")]
        [JsonProperty("other-subfields")]
        public SubField[] OtherSubFields { get; set; }

        /// <summary>
        /// MFN записи, из которой заимствован экземпляр.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Краткое библиографическое описание экземпляра.
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

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

        #region Private members

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>ExemplarInfo.</returns>
        public static ExemplarInfo Parse
            (
                RecordField field
            )
        {
            ExemplarInfo result = new ExemplarInfo
                {
                    Status = field.GetSubFieldText ( 'a', 0 ),
                    Number = field.GetSubFieldText ( 'b', 0 ),
                    Date = field.GetSubFieldText ( 'c', 0 ),
                    Place = field.GetSubFieldText ( 'd', 0 ),
                    Collection = field.GetSubFieldText ( 'q', 0 ),
                    ShelfIndex = field.GetSubFieldText ( 'r', 0 ),
                    Price = field.GetSubFieldText ( 'e', 0 ),
                    Barcode = field.GetSubFieldText ( 'h', 0 ),
                    Amount = field.GetSubFieldText ( '1', 0 ),
                    Purpose = field.GetSubFieldText ( 't', 0 ),
                    Coefficient = field.GetSubFieldText ( '=', 0 ),
                    OffBalance = field.GetSubFieldText ( '4', 0 ),
                    KsuNumber1 = field.GetSubFieldText ( 'u', 0 ),
                    ActNumber1 = field.GetSubFieldText ( 'y', 0 ),
                    Channel = field.GetSubFieldText ( 'f', 0 ),
                    OnHand = field.GetSubFieldText ( '2', 0 ),
                    ActNumber2 = field.GetSubFieldText ( 'v', 0 ),
                    WriteOff = field.GetSubFieldText ( 'x', 0 ),
                    Completion = field.GetSubFieldText ( 'k', 0 ),
                    ActNumber3 = field.GetSubFieldText ( 'w', 0 ),
                    Moving = field.GetSubFieldText ( 'z', 0 ),
                    NewPlace = field.GetSubFieldText ( 'm', 0 ),
                    CheckedDate = field.GetSubFieldText ( 's', 0 ),
                    CheckedAmount = field.GetSubFieldText ( '0', 0 ),
                    RealPlace = field.GetSubFieldText ( '!', 0 ),
                    BindingIndex = field.GetSubFieldText('p', 0),
                    BindingNumber = field.GetSubFieldText('i',0),
                    OtherSubFields = field.SubFields
                        .Where(sub => KnownCodes
                            .IndexOf(char.ToLower(sub.Code)) < 0)
                        .ToArray()
                };
            return result;
        }

        /// <summary>
        /// Разбор записи на экземпляры.
        /// </summary>
        public static ExemplarInfo[] Parse
            (
                IrbisRecord record,
                string tagNumber
            )
        {
            ExemplarInfo[] result = record.Fields
                .GetField(tagNumber)
                .Select(Parse)
                .ToArray();

            foreach (ExemplarInfo exemplar in result)
            {
                exemplar.Mfn = record.Mfn;
                exemplar.Description = record.Description;
            }

            return result;
        }

        /// <summary>
        /// Разбор записи на экземпляры.
        /// </summary>
        public static ExemplarInfo[] Parse
            (
                IrbisRecord record
            )
        {
            return Parse
                (
                    record,
                    ExemplarTag
                );
        }

        /// <summary>
        /// Преобразование экземпляра обратно в поле записи.
        /// </summary>
        public RecordField ToField ()
        {
            RecordField result = new RecordField("910")
                .AddNonEmptySubField ( 'a', Status )
                .AddNonEmptySubField ( 'b', Number )
                .AddNonEmptySubField ( 'c', Date )
                .AddNonEmptySubField ( 'd', Place )
                .AddNonEmptySubField ( 'q', Collection )
                .AddNonEmptySubField ( 'r', ShelfIndex )
                .AddNonEmptySubField ( 'e', Price )
                .AddNonEmptySubField ( 'h', Barcode )
                .AddNonEmptySubField ( '1', Amount )
                .AddNonEmptySubField ( 't', Purpose )
                .AddNonEmptySubField ( '=', Coefficient )
                .AddNonEmptySubField ( '4', OffBalance )
                .AddNonEmptySubField ( 'u', KsuNumber1 )
                .AddNonEmptySubField ( 'y', ActNumber1 )
                .AddNonEmptySubField ( 'f', Channel )
                .AddNonEmptySubField ( '2', OnHand )
                .AddNonEmptySubField ( 'v', ActNumber2 )
                .AddNonEmptySubField ( 'x', WriteOff )
                .AddNonEmptySubField ( 'k', Completion )
                .AddNonEmptySubField ( 'w', ActNumber3 )
                .AddNonEmptySubField ( 'z', Moving )
                .AddNonEmptySubField ( 'm', NewPlace )
                .AddNonEmptySubField ( 's', CheckedDate )
                .AddNonEmptySubField ( '0', CheckedAmount )
                .AddNonEmptySubField ( '!', RealPlace )
                .AddNonEmptySubField ( 'p', BindingIndex )
                .AddNonEmptySubField ( 'i', BindingNumber );

            if (OtherSubFields != null)
            {
                foreach (SubField subField in OtherSubFields)
                {
                    result.AddSubField(subField.Code, subField.Text);
                }
            }

            return result;
        }

        /// <summary>
        /// Compares two specified numbers.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>System.Int32.</returns>
        public static int CompareNumbers
            (
                ExemplarInfo first,
                ExemplarInfo second
            )
        {
            NumberText one = new NumberText(first.Number);
            NumberText two = new NumberText(second.Number);
            return one.CompareTo(two);
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            string result = string.Format
                (
                    "{0} ({1}) [{2}]",
                    Number,
                    Place,
                    Status
                );

            if (!string.IsNullOrEmpty(BindingNumber))
            {
                result = result + " <подшивка " +  BindingNumber + ">";
            }

            return result;
        }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderRegistration.cs -- информация о регистрации/перерегистрации читателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

using ManagedClient.Mapping;

#endregion

namespace ManagedClient.Readers
{
    /// <summary>
    /// Информация о регистрации/перерегистрации читателя.
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    [XmlRoot("registration")]
    public sealed class ReaderRegistration
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Поле регистрация.
        /// </summary>
        public const string RegistrationTag = "51";

        /// <summary>
        /// Поле "перерегистрация".
        /// </summary>
        public const string ReregistrationTag = "52";

        #endregion

        #region Properties

        /// <summary>
        /// Дата. Подполе *.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        public string DateString { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime Date
        {
            get
            {
                return IrbisDate.ConvertStringToDate(DateString);
            }
            set
            {
                DateString = IrbisDate.ConvertDateToString(value);
            }
        }

        /// <summary>
        /// Место (кафедра обслуживания).
        /// Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("chair")]
        [JsonProperty("chair")]
        public string Chair { get; set; }

        /// <summary>
        /// Номер приказа. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("order-number")]
        [JsonProperty("order-number")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Причина. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("reason")]
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Ссылка на зарегистрированного читателя.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReaderInfo Reader { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [NotNull]
        public static ReaderRegistration Parse
            (
                [NotNull]RecordField field
            )
        {
            ReaderRegistration result = new ReaderRegistration
            {
                DateString = field.Text,
                Chair = field.GetFirstSubFieldText('c'),
                OrderNumber = field.GetFirstSubFieldText('a'),
                Reason = field.GetFirstSubFieldText('b')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ReaderRegistration[] Parse
            (
                [NotNull] IrbisRecord record,
                [NotNull] string tag
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

            ReaderRegistration[] result = record.Fields
                .GetField(tag)
                .Select(Parse)
                .ToArray();

            return result;
        }

        /// <summary>
        /// Преобразование в поле.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField
                {
                    Text = DateString
                }
                .AddNonEmptySubField('c', Chair)
                .AddNonEmptySubField('a', OrderNumber)
                .AddNonEmptySubField('b', Reason);

            return result;
        }

        #region Ручная сериализация

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(DateString);
            writer.WriteNullable(Chair);
            writer.WriteNullable(OrderNumber);
            writer.WriteNullable(Reason);
        }

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        [NotNull]
        public static ReaderRegistration ReadFromStream
            (
                [NotNull] BinaryReader reader
            )
        {
            ReaderRegistration result = new ReaderRegistration
            {
                DateString = reader.ReadNullableString(),
                Chair = reader.ReadNullableString(),
                OrderNumber = reader.ReadNullableString(),
                Reason = reader.ReadNullableString()
            };

            return result;
        }

        #endregion

        #endregion
    }
}

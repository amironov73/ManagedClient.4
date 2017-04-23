﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderAddress.cs -- адрес читателя
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Readers
{
    /// <summary>
    /// Адрес читателя: поле 13 в базе RDR.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [XmlRoot("address")]
    public sealed class ReaderAddress
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "13";

        #endregion

        #region Properties

        /// <summary>
        /// Почтовый индекс. Подполе A.
        /// </summary>
        [XmlAttribute("postcode")]
        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        /// <summary>
        /// Страна/республика. Подполе B.
        /// </summary>
        [XmlAttribute("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Город. Подполе C.
        /// </summary>
        [XmlAttribute("city")]
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// Улица. Подполе D.
        /// </summary>
        [XmlAttribute("street")]
        [JsonProperty("street")]
        public string Street { get; set; }

        /// <summary>
        /// Номер дома. Подполе E.
        /// </summary>
        [XmlAttribute("building")]
        [JsonProperty("building")]
        public string Building { get; set; }

        /// <summary>
        /// Номер подъезда. Подполе G.
        /// </summary>
        [XmlAttribute("entrance")]
        [JsonProperty("entrance")]
        public string Entrance { get; set; }

        /// <summary>
        /// Номер квартиры. Подполе H.
        /// </summary>
        [XmlAttribute("apartment")]
        [JsonProperty("apartment")]
        public string Apartment { get; set; }

        /// <summary>
        /// Дополнительные данные. Подполе F.
        /// </summary>
        [XmlAttribute("additional-data")]
        [JsonProperty("additional-data")]
        public string AdditionalData { get; set; }

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
        /// Разбор поля 13.
        /// </summary>
        [CanBeNull]
        public static ReaderAddress Parse
            (
                [CanBeNull]RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            return new ReaderAddress
            {
                Postcode = field.GetFirstSubFieldText('A'),
                Country = field.GetFirstSubFieldText('B'),
                City = field.GetFirstSubFieldText('C'),
                Street = field.GetFirstSubFieldText('D'),
                Building = field.GetFirstSubFieldText('E'),
                Entrance = field.GetFirstSubFieldText('G'),
                Apartment = field.GetFirstSubFieldText('H'),
                AdditionalData = field.GetFirstSubFieldText('F')
            };
        }

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        [CanBeNull]
        public static ReaderAddress Parse
            (
                [NotNull]IrbisRecord record,
                [NotNull]string tag
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

            RecordField field = record.Fields
                .GetField(tag)
                .FirstOrDefault();

            return ReferenceEquals(field, null)
                ? null
                : Parse(field);
        }

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        [CanBeNull]
        public static ReaderAddress Parse
            (
                [NotNull]IrbisRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
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
            writer.WriteNullable(Postcode);
            writer.WriteNullable(Country);
            writer.WriteNullable(City);
            writer.WriteNullable(Street);
            writer.WriteNullable(Building);
            writer.WriteNullable(Entrance);
            writer.WriteNullable(Apartment);
            writer.WriteNullable(AdditionalData);
        }

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        public static ReaderAddress ReadFromStream
            (
                [NotNull] BinaryReader reader
            )
        {
            ReaderAddress result = new ReaderAddress
            {
                Postcode = reader.ReadNullableString(),
                Country = reader.ReadNullableString(),
                City = reader.ReadNullableString(),
                Street = reader.ReadNullableString(),
                Building = reader.ReadNullableString(),
                Entrance = reader.ReadNullableString(),
                Apartment = reader.ReadNullableString(),
                AdditionalData = reader.ReadNullableString()
            };

            return result;
        }

        #endregion

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
            string[] list = new List<string>
            {
                Postcode,
                Country,
                City,
                Street,
                Building,
                Entrance,
                Apartment,
                AdditionalData
            }
                .NonNullItems()
                .ToArray();

            return string.Join
                (
                    ", ",
                    list
                );
        }

        #endregion
    }
}

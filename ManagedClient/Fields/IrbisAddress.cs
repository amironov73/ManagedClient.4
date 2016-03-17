/* IrbisAddress.cs -- адрес читателя
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Адрес читателя: поле 13 в базе RDR.
    /// </summary>
    [Serializable]
    public sealed class IrbisAddress
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
        public string Postcode { get; set; }

        /// <summary>
        /// Страна/республика. Подполе B.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Город. Подполе C.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Улица. Подполе D.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Номер дома. Подполе E.
        /// </summary>
        public string Building { get; set; }

        /// <summary>
        /// Номер подъезда. Подполе G.
        /// </summary>
        public string Entrance { get; set; }

        /// <summary>
        /// Номер квартиры. Подполе H.
        /// </summary>
        public string Apartment { get; set; }

        /// <summary>
        /// Дополнительные данные. Подполе F.
        /// </summary>
        public string AdditionalData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        public static IrbisAddress Parse
            (
                RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            return new IrbisAddress
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
        public static IrbisAddress Parse
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
        public static IrbisAddress Parse
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

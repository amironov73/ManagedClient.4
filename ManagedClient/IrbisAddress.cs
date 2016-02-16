/* IrbisAddress.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient
{
    [Serializable]
    public sealed class IrbisAddress
    {
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

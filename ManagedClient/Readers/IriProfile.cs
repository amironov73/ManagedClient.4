/* IriProfile.cs -- профиль ИРИ
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JetBrains.Annotations;

#endregion

namespace ManagedClient.Readers
{
    /// <summary>
    /// Профиль ИРИ
    /// </summary>
    [PublicAPI]
    [Serializable]
    [XmlRoot("iri-profile")]
    public sealed class IriProfile
    {
        #region Constants

        /// <summary>
        /// Тег поля ИРИ.
        /// </summary>
        public const string IriTag = "140";
        
        #endregion

        #region Properties

        /// <summary>
        /// Подполе A
        /// </summary>
        [XmlAttribute("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Подполе B
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// Подполе C
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Подполе D
        /// </summary>
        [XmlAttribute("query")]
        public string Query { get; set; }

        /// <summary>
        /// Подполе E
        /// </summary>
        public int Periodicity { get; set; }

        /// <summary>
        /// Подполе F
        /// </summary>
        public string LastServed { get; set; }

        /// <summary>
        /// Подполе I
        /// </summary>
        public string Database { get; set; }

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
        public static IriProfile ParseField
            (
                [NotNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                throw new ArgumentNullException("field");
            }

            IriProfile result = new IriProfile
            {
                Active = field.GetFirstSubFieldText('a') == "1",
                ID = field.GetFirstSubFieldText('b'),
                Title = field.GetFirstSubFieldText('c'),
                Query = field.GetFirstSubFieldText('d'),
                Periodicity = int.Parse(field.GetFirstSubFieldText('e')),
                LastServed = field.GetFirstSubFieldText('f'),
                Database = field.GetFirstSubFieldText('i')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        public static IriProfile[] ParseRecord
            (
                [NotNull] IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            List<IriProfile> result = new List<IriProfile>();
            foreach (RecordField field in record.Fields
                .GetField(IriTag))
            {
                IriProfile profile = ParseField(field);
                result.Add(profile);
            }
            return result.ToArray();
        }

        #endregion
    }
}

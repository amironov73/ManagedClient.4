﻿/* IriProfile.cs -- профиль ИРИ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using JetBrains.Annotations;

using Newtonsoft.Json;

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
        : IHandmadeSerializable
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
        [CanBeNull]
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// Подполе C
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Подполе D
        /// </summary>
        [CanBeNull]
        [XmlAttribute("query")]
        public string Query { get; set; }

        /// <summary>
        /// Подполе E
        /// </summary>
        public int Periodicity { get; set; }

        /// <summary>
        /// Подполе F
        /// </summary>
        [CanBeNull]
        public string LastServed { get; set; }

        /// <summary>
        /// Подполе I
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }


        /// <summary>
        /// Ссылка на читателя.
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

        #region Ручная сериализация

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Active);
            writer.WriteNullable(ID);
            writer.WriteNullable(Title);
            writer.WriteNullable(Query);
            writer.WritePackedInt32(Periodicity);
            writer.WriteNullable(LastServed);
            writer.WriteNullable(Database);
        }

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public static void SaveToFile
            (
                [NotNull] string fileName,
                [NotNull][ItemNotNull] IriProfile[] profiles
            )
        {
            profiles.SaveToFile(fileName);
        }
        
        /// <summary>
        /// Чтение из потока.
        /// </summary>
        public static IriProfile ReadFromStream
            (
                [NotNull] BinaryReader reader
            )
        {
            IriProfile result = new IriProfile
            {
                Active = reader.ReadBoolean(),
                ID = reader.ReadNullableString(),
                Title = reader.ReadNullableString(),
                Query = reader.ReadNullableString(),
                Periodicity = reader.ReadPackedInt32(),
                LastServed = reader.ReadNullableString(),
                Database = reader.ReadNullableString()
            };

            return result;
        }

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static IriProfile[] ReadProfilesFromFile
            (
                [NotNull] string fileName
            )
        {
            IriProfile[] result = IrbisIOUtils.ReadFromFile
                (
                    fileName,
                    ReadFromStream
                );

            return result;
        }

        #endregion

        #endregion
    }
}

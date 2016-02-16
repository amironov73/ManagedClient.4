/* FieldDefect.cs -- дефект в поле/подполе.
 */

#region Using directives

using System;
using System.Xml.Serialization;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Дефект в поле/подполе.
    /// </summary>
    [Serializable]
    [XmlRoot("defect")]
    public sealed class FieldDefect
    {
        #region Properties

        /// <summary>
        /// Поле.
        /// </summary>
        [JsonProperty("field")]
        [XmlAttribute("field")]
        public string Field { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [JsonProperty("field-repeat")]
        [XmlAttribute("field-repeat")]
        public int FieldRepeat { get; set; }

        /// <summary>
        /// Подполе (если есть).
        /// </summary>
        [JsonProperty("subfield")]
        [XmlAttribute("subfield")]
        public string Subfield { get; set; }

        /// <summary>
        /// Значение поля/подполя.
        /// </summary>
        [JsonProperty("value")]
        [XmlAttribute("value")]
        public string Value { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        [JsonProperty("message")]
        [XmlAttribute("message")]
        public string Message { get; set; }

        /// <summary>
        /// Урон от дефекта.
        /// </summary>
        [JsonProperty("damage")]
        [XmlAttribute("damage")]
        public int Damage { get; set; }

        #endregion
    }
}

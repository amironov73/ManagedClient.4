/* GblItem.cs
 */

#region Using directives

using System;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace ManagedClient.Gbl
{
    /// <summary>
    /// Элемент глобальной корректировки.
    /// </summary>
    [Serializable]
    [XmlRoot("gbl-item")]
    public sealed class GblItem
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов
        /// </summary>
        public const string Delimiter = "\x1F\x1E";

        #endregion

        #region Properties

        /// <summary>
        /// Команда (оператор), например, ADD или DEL.
        /// </summary>
        [XmlElement("command")]
        public string Command { get; set; }

        /// <summary>
        /// Первый параметр, как правило, спецификация поля/подполя.
        /// </summary>
        [XmlElement("parameter1")]
        public string Parameter1 { get; set; }

        /// <summary>
        /// Второй параметр, как правило, спецификация повторения.
        /// </summary>
        [XmlElement("parameter2")]
        public string Parameter2 { get; set; }

        /// <summary>
        /// Первый формат, например, выражение для замены.
        /// </summary>
        [XmlElement("format1")]
        public string Format1 { get; set; }

        /// <summary>
        /// Второй формат, например, заменяющее выражение.
        /// </summary>
        [XmlElement("format2")]
        public string Format2 { get; set; }

        #endregion

        #region Public methods
        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(Command);
            result.Append(Delimiter);
            result.Append(Parameter1);
            result.Append(Delimiter);
            result.Append(Parameter2);
            result.Append(Delimiter);
            result.Append(Format1);
            result.Append(Delimiter);
            result.Append(Format2);
            result.Append(Delimiter);

            return result.ToString();
        }

        #endregion
    }
}

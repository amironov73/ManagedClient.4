/* SubField.cs
 */

#region Using directives

using System;
using System.Xml.Serialization;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Подполе MARC-записи.
    /// </summary>
    [Serializable]
    [XmlRoot("subfield")]
    public class SubField
    {
        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public char Code { get; set; }

        /// <summary>
        /// Код подполя.
        /// </summary>
        /// <remarks>
        /// Для XML-сериализации.
        /// </remarks>
        [XmlAttribute("code")]
        [JsonProperty("code")]
        public string CodeString
        {
            get { return Code.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Code = value[0];
                }
            }
        }

        /// <summary>
        /// Значение подполя.
        /// </summary>
        [XmlAttribute("text")]
        [JsonProperty("text")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _text = value;
                }
                else
                {
                    if (value.IndexOf(RecordField.Delimiter)>=0)
                    {
                        throw new ArgumentException
                            (
                                "Contains delimiter",
                                "Text"
                            );
                    }
                    _text = value;
                }
            }
        }

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

        /// <summary>
        /// Ссылка на поле, владеющее
        /// данным подполем. Настраивается
        /// перед передачей в скрипты.
        /// Всё остальное время неактуально.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [NonSerialized]
        public RecordField Field;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SubField" /> class.
        /// </summary>
        public SubField()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubField" /> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public SubField(char code)
        {
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubField" /> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="text">The text.</param>
        public SubField(char code, string text)
        {
            Code = code;
            Text = text;
        }

        #endregion

        #region Private members

        private string _text;

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public SubField Clone ( )
        {
            SubField result = new SubField
                                  {
                                      Code = Code,
                                      Text = Text
                                  };
            return result;
        }

        /// <summary>
        /// Compares the specified sub field1.
        /// </summary>
        /// <param name="subField1">The sub field1.</param>
        /// <param name="subField2">The sub field2.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns>System.Int32.</returns>
        public static int Compare
            (
                SubField subField1, 
                SubField subField2,
                bool verbose
            )
        {
            int result = subField1.Code.CompareTo(subField2.Code);
            if (result != 0)
            {
                if (verbose)
                {
                    Console.WriteLine
                        (
                            "SubField1 Code={0}, SubField2 Code={1}",
                            subField1.Code,
                            subField2.Code
                        );
                }
                return result;
            }
            
            result = string.CompareOrdinal(subField1.Text, subField2.Text);
            if (verbose && (result != 0))
            {
                Console.WriteLine
                    (
                        "SubField1 Text={0}, SubField2 Text={1}",
                        subField1.Text,
                        subField2.Text
                    );
            }
            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format
                (
                    "^{0}{1}",
                    Code,
                    Text
                );
        }

        #endregion
    }
}

/* RecordField.cs - поле библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using JetBrains.Annotations;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Поле библиографической записи.
    /// </summary>
    [Serializable]
    [XmlRoot("field")]
    [MoonSharpUserData]
    [DebuggerDisplay("Tag={Tag} Text={Text}")]
    public sealed class RecordField
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Разделитель подполей.
        /// </summary>
        public const char Delimiter = '^';

        #endregion

        #region Properties

        /// <summary>
        /// Флаг: выбрасывать исключение, если свойству Text
        /// присваивается значение, содержащее разделитель.
        /// </summary>
        public static bool BreakOnTextContainDelimiters;

        /// <summary>
        /// Метка поля.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// Настраивается перед передачей
        /// в скрипты.
        /// Не используется в большинстве сценариев.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [NonSerialized]
        public int Repeat;

        /// <summary>
        /// Значение поля до первого разделителя подполей.
        /// </summary>
        /// <remarks>
        /// Внимание! Если присваиваемое значение содержит
        /// разделитель, то происходит и присвоение подполей!
        /// Имеющиеся в SubFields значения при этом пропадают
        /// и замещаются на вновь присваиваемые!
        /// </remarks>
        [CanBeNull]
        [XmlAttribute("text")]
        [JsonProperty("text")]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _text = value;
                }
                else
                {
                    if (value.IndexOf(Delimiter) >= 0)
                    {
                        if (BreakOnTextContainDelimiters)
                        {
                            throw new ArgumentException
                                (
                                    "Contains delimiter",
                                    "value"
                                );
                        }
                        SetSubFields(value);
                    }
                    else
                    {
                        _text = value;
                    }
                }
            }
        }

        /// <summary>
        /// Список подполей.
        /// </summary>
        [XmlElement("subfield")]
        [JsonProperty("subfields")]
        public SubFieldCollection SubFields
        {
            get { return _subFields; }
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
        /// Ссылка на запись, владеющую
        /// данным полем. Настраивается
        /// перед передачей в скрипты.
        /// Всё остальное время неактуально.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [NonSerialized]
        public IrbisRecord Record;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordField" /> class.
        /// </summary>
        public RecordField()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordField" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public RecordField
            (
                string tag
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordField" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="text">The text.</param>
        public RecordField
            (
                string tag,
                string text
            )
        {
            Tag = tag;
            Text = text;
        }

        #endregion

        #region Private members

        private string _text;

        private readonly SubFieldCollection _subFields
            = new SubFieldCollection();

        [NonSerialized]
        private object _userData;

        private static void AddSubField
            (
                RecordField field,
                char code,
                StringBuilder value
            )
        {
            if (code != 0)
            {
                if (value.Length == 0)
                {
                    field.SubFields.Add(new SubField(code));
                }
                else
                {
                    field.SubFields.Add(new SubField
                        (
                            code,
                            value.ToString()
                        ));
                }
            }
            value.Length = 0;
        }

        private static void _EncodeSubField
            (
                StringBuilder builder,
                SubField subField
            )
        {
            builder.AppendFormat
                (
                    "{0}{1}{2}",
                    Delimiter,
                    subField.Code,
                    subField.Text
                );
        }

        internal static void _EncodeField
            (
                StringBuilder builder,
                RecordField field
            )
        {
            int fieldNumber = field.Tag.SafeParseInt32();
            if (fieldNumber != 0)
            {
                builder.AppendFormat
                    (
                        "{0}#",
                        fieldNumber
                    );
            }
            else
            {
                builder.AppendFormat
                    (
                        "{0}#",
                        field.Tag
                    );
            }

            if (!string.IsNullOrEmpty(field.Text))
            {
                builder.Append(field.Text);
            }

            foreach (SubField subField in field.SubFields)
            {
                _EncodeSubField
                    (
                        builder,
                        subField
                    );
            }
            builder.Append("\x001F\x001E");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Перечень подполей с указанным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <remarks>Сравнение кодов происходит без учета
        /// регистра символов.</remarks>
        /// <returns>Найденные подполя.</returns>
        public SubField[] GetSubField
            (
                char code
            )
        {
            SubField[] result = SubFields
                .Where(_ => _.Code.SameChar(code))
                .ToArray();
            return result;
        }

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Найденное подполе или <c>null</c>.</returns>
        public SubField GetSubField
            (
                char code,
                int occurrence
            )
        {
            SubField[] found = GetSubField(code);
            occurrence = (occurrence >= 0)
                            ? occurrence
                            : found.Length - occurrence;
            SubField result = null;
            if ((occurrence >= 0) && (occurrence < found.Length))
            {
                result = found[occurrence];
            }
            return result;
        }

        /// <summary>
        /// Gets the first subfield.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>SubField.</returns>
        public SubField GetFirstSubField
            (
                char code
            )
        {
            return SubFields.FirstOrDefault(sf => sf.Code.SameChar(code));
        }

        /// <summary>
        /// Получение текста указанного подполя.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Текст найденного подполя или <c>null</c>.</returns>
        public string GetSubFieldText
            (
                char code,
                int occurrence
            )
        {
            SubField result = GetSubField(code, occurrence);
            return (result == null)
                       ? null
                       : result.Text;
        }

        /// <summary>
        /// Gets first subfield text.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>System.String.</returns>
        public string GetFirstSubFieldText
            (
                char code
            )
        {
            SubField result = GetFirstSubField(code);
            return (result == null)
                ? null
                : result.Text;
        }

        /// <summary>
        /// Filters the sub fields.
        /// </summary>
        /// <param name="subFields">The sub fields.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>SubField[].</returns>
        public static SubField[] FilterSubFields
            (
                IEnumerable<SubField> subFields,
                Func<SubField, bool> predicate
            )
        {
            return subFields
                .Where(predicate)
                .ToArray();
        }

        /// <summary>
        /// Filters the sub fields.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>SubField[].</returns>
        public SubField[] FilterSubFields
            (
                Func<SubField, bool> predicate
            )
        {
            return FilterSubFields(SubFields, predicate);
        }

        /// <summary>
        /// Filters the sub fields.
        /// </summary>
        /// <param name="subFields">The sub fields.</param>
        /// <param name="codes">The codes.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>SubField[].</returns>
        public static SubField[] FilterSubFields
            (
                IEnumerable<SubField> subFields,
                char[] codes,
                Func<SubField, bool> predicate
            )
        {
            return subFields
                .Where(_ => _.Code.OneOf(codes)
                            && predicate(_))
                .ToArray();
        }

        /// <summary>
        /// Filters the sub fields.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>SubField[].</returns>
        public SubField[] FilterSubFields
            (
                char[] codes,
                Func<SubField, bool> predicate
            )
        {
            return FilterSubFields(SubFields, codes, predicate);
        }

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        /// <param name="subFields"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        public static SubField[] FilterSubFields
            (
                IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            return subFields
                .Where(_ => _.Code.OneOf(codes))
                .ToArray();
        }

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public SubField[] FilterSubFields
            (
                params char[] codes
            )
        {
            return FilterSubFields(SubFields, codes);
        }

        /// <summary>
        /// Adds the sub field.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="text">The text.</param>
        /// <returns>RecordField.</returns>
        public RecordField AddSubField
            (
                char code,
                string text
            )
        {
            SubFields.Add(new SubField(code, text));
            return this;
        }

        /// <summary>
        /// Adds the non empty sub field.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="text">The text.</param>
        /// <returns>RecordField.</returns>
        public RecordField AddNonEmptySubField
            (
                char code,
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                AddSubField(code, text);
            }
            return this;
        }

        /// <summary>
        /// Sets the sub field.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <remarks>Устанавливает значение только первого
        /// подполя с указанным кодом (если в поле их несколько)!
        /// </remarks>
        public RecordField SetSubField
            (
                char code,
                string text
            )
        {
            SubField subField = SubFields
                .Find(_ => _.Code.SameChar(code));
            if (subField == null)
            {
                subField = new SubField(code, text);
                SubFields.Add(subField);
            }
            subField.Text = text;
            return this;
        }

        /// <summary>
        /// Replaces the sub field.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>RecordField.</returns>
        public RecordField ReplaceSubField
            (
                char code,
                string oldValue,
                string newValue
            )
        {
            var found = SubFields
                .Where(sf => sf.Code.SameChar(code)
                && (string.CompareOrdinal(sf.Text, oldValue) == 0));
            foreach (SubField subField in found)
            {
                subField.Text = newValue;
            }
            return this;
        }

        /// <summary>
        /// Removes the sub field.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <remarks>Удаляет все повторения подполей
        /// с указанным кодом.
        /// </remarks>
        public RecordField RemoveSubField
            (
                char code
            )
        {
            code = char.ToLowerInvariant(code);
            SubField[] found = SubFields
                .FindAll(_ => char.ToLowerInvariant(_.Code) == code)
                .ToArray();

            foreach (SubField subField in found)
            {
                SubFields.Remove(subField);
            }

            return this;
        }

        /// <summary>
        /// Replaces the sub field.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool ReplaceSubField
            (
                char code,
                string newValue,
                bool ignoreCase
            )
        {
            string oldValue = GetSubFieldText(code, 0);
            bool changed = string.Compare
                (
                    oldValue,
                    newValue,
                    ignoreCase
                ) != 0;

            if (changed)
            {
                SetSubField(code, newValue);
            }

            return changed;

        }

        /// <summary>
        /// Haves the sub field.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool HaveSubField
            (
                params char[] codes
            )
        {
            return (codes.Any(code => GetSubField(code).Length != 0));
        }

        /// <summary>
        /// Haves the not sub field.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool HaveNotSubField
            (
                params char[] codes
            )
        {
            return (codes.All(code => GetSubField(code).Length == 0));
        }

        /// <summary>
        /// To the text.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ToText()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(Text))
            {
                result.Append(Text);
            }
            foreach (SubField subField in SubFields)
            {
                string subText = subField.ToString();
                if (!string.IsNullOrEmpty(subText))
                {
                    result.Append(subText);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Вывод поля в порядке алфавита
        /// кодов подполей.
        /// </summary>
        /// <returns></returns>
        public string ToSortedText()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(Text))
            {
                result.Append(Text);
            }
            foreach (SubField subField 
                in SubFields.OrderBy(sf => sf.CodeString.ToUpperInvariant()))
            {
                string subText = string.Format
                    (
                        "^{0}{1}",
                        subField.CodeString.ToUpperInvariant(),
                        subField.Text
                    );
                if (!string.IsNullOrEmpty(subText))
                {
                    result.Append(subText);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Парсинг текстового проедставления поля
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static RecordField Parse
            (
                string tag,
                string body
            )
        {
            RecordField result = new RecordField(tag);

            int first = body.IndexOf(Delimiter);
            if (first != 0)
            {
                if (first < 0)
                {
                    result.Text = body;
                    body = string.Empty;
                }
                else
                {
                    result.Text = body.Substring
                        (
                            0,
                            first
                        );
                    body = body.Substring(first);
                }
            }

            var code = (char)0;
            var value = new StringBuilder();
            foreach (char c in body)
            {
                if (c == Delimiter)
                {
                    AddSubField
                        (
                            result,
                            code,
                            value
                        );
                    code = (char)0;
                }
                else
                {
                    if (code == 0)
                    {
                        code = c;
                    }
                    else
                    {
                        value.Append(c);
                    }
                }
            }

            AddSubField
                (
                    result,
                    code,
                    value
                );

            return result;
        }

        /// <summary>
        /// Reparses the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>RecordField.</returns>
        public RecordField Reparse
            (
                string text
            )
        {
            RecordField parsed = Parse
                (
                    Tag,
                    text
                );
            if (!ReferenceEquals(parsed, null))
            {
                Text = parsed.Text;
                _subFields.Clear();
                _subFields.AddRange(parsed._subFields);
            }
            return this;
        }

        /// <summary>
        /// Adds the sub fields.
        /// </summary>
        /// <param name="subFields">The sub fields.</param>
        /// <returns>RecordField.</returns>
        public RecordField AddSubFields
            (
                IEnumerable<SubField> subFields
            )
        {
            SubFields.AddRange
                (
                    subFields.NonNullItems()
                );

            return this;
        }

        /// <summary>
        /// Adds the sub fields.
        /// </summary>
        /// <param name="encodedText">The encoded text.</param>
        /// <returns>RecordField.</returns>
        public RecordField AddSubFields
            (
                string encodedText
            )
        {
            RecordField parsed = Parse(encodedText);
            if (!ReferenceEquals(parsed, null))
            {

                if (!string.IsNullOrEmpty(parsed.Text))
                {
                    Text += parsed.Text;
                }
                AddSubFields(parsed.SubFields);
            }
            return this;
        }

        /// <summary>
        /// Sets the sub fields.
        /// </summary>
        /// <param name="subFields">The sub fields.</param>
        /// <returns>RecordField.</returns>
        public RecordField SetSubFields
            (
                IEnumerable<SubField> subFields
            )
        {
            foreach (SubField subField in subFields.NonNullItems())
            {
                SetSubField
                    (
                        subField.Code,
                        subField.Text
                    );
            }

            return this;
        }

        /// <summary>
        /// Sets the sub fields.
        /// </summary>
        /// <param name="encodedText">The encoded text.</param>
        /// <returns>RecordField.</returns>
        public RecordField SetSubFields
            (
                string encodedText
            )
        {
            RecordField parsed = Parse(encodedText);
            if (!ReferenceEquals(parsed, null))
            {
                if (!string.IsNullOrEmpty(parsed.Text))
                {
                    _text = parsed.Text;
                }
                SetSubFields(parsed.SubFields);
            }
            return this;
        }

        /// <summary>
        /// Парсинг строкового представления поля.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        public static RecordField Parse
            (
                string line
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }
            string[] parts = line.SplitFirst('#');
            string tag = parts[0];
            string body = parts[1];
            return Parse
                (
                    tag,
                    body
                );
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>RecordField.</returns>
        public RecordField Clone()
        {
            RecordField result = new RecordField
                                     {
                                         Tag = Tag,
                                         Text = Text
                                     };

            result.SubFields.AddRange
                (
                    from subField in SubFields
                    select subField.Clone()
                );

            return result;
        }

        /// <summary>
        /// Compares the specified field1.
        /// </summary>
        /// <param name="field1">The field1.</param>
        /// <param name="field2">The field2.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns>System.Int32.</returns>
        public static int Compare
            (
                RecordField field1,
                RecordField field2,
                bool verbose
            )
        {
            int result = string.CompareOrdinal(field1.Tag, field2.Tag);
            if (result != 0)
            {
                if (verbose)
                {
                    Console.WriteLine
                        (
                            "Field1 Tag={0}, Field2 Tag={1}",
                            field1.Tag,
                            field2.Tag
                        );
                }
                return result;
            }

            if (!string.IsNullOrEmpty(field1.Text)
                || !string.IsNullOrEmpty(field2.Text))
            {
                result = string.CompareOrdinal(field1.Text, field2.Text);
                if (result != 0)
                {
                    if (verbose)
                    {
                        Console.WriteLine
                            (
                                "Field1 Text={0}, Field2 Text={1}",
                                field1.Text,
                                field2.Text
                            );
                    }
                    return result;
                }
            }

            IEnumerator<SubField> enum1 = field1.SubFields.GetEnumerator();
            IEnumerator<SubField> enum2 = field2.SubFields.GetEnumerator();

            while (true)
            {
                bool next1 = enum1.MoveNext();
                bool next2 = enum2.MoveNext();

                if ((!next1) && (!next2))
                {
                    break;
                }
                if (!next1)
                {
                    return -1;
                }
                if (!next2)
                {
                    return 1;
                }

                SubField subField1 = enum1.Current;
                SubField subField2 = enum2.Current;

                result = SubField.Compare
                    (
                        subField1,
                        subField2,
                        verbose
                    );
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets the embedded fields.
        /// </summary>
        /// <param name="sign">The sign.</param>
        public RecordField[] GetEmbeddedFields
            (
                char sign
            )
        {
            List<RecordField> result = new List<RecordField>();

            RecordField found = null;

            foreach (SubField subField in SubFields)
            {
                if (subField.Code == sign)
                {
                    if (found != null)
                    {
                        result.Add(found);
                    }
                    string tag = subField.Text.Substring
                        (
                            0,
                            3
                        );
                    found = new RecordField(tag);
                    if (tag.StartsWith("00")
                        && (subField.Text.Length > 3)
                       )
                    {
                        found.Text = subField.Text.Substring(3);
                    }
                }
                else
                {
                    if (found != null)
                    {
                        found.AddSubField
                            (
                                subField.Code,
                                subField.Text
                            );
                    }
                }
            }

            if (found != null)
            {
                result.Add(found);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets the embedded fields.
        /// </summary>
        public RecordField[] GetEmbeddedFields()
        {
            return GetEmbeddedFields('1');
        }

        #region Ручная сериализация

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        /// <param name="writer"></param>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Tag);
            writer.WriteNullable(Text);
            SubFields.SaveToStream(writer);
        }

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [NotNull]
        public static RecordField ReadFromStream
            (
                [NotNull] BinaryReader reader
            )
        {
            RecordField result = new RecordField
            {
                Tag = reader.ReadNullableString(),
                Text = reader.ReadNullableString()
            };
            result.SubFields.ReadFromStream(reader);

            return result;
        }

        #endregion

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
            StringBuilder result = new StringBuilder();
            _EncodeField(result, this);
            return result.ToString();
        }

        #endregion
    }
}

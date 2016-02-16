/* IrbisRecord.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// MARC-запись.
    /// </summary>
    [Serializable]
    public sealed class IrbisRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Статус записи: удалена, блокирована и т.д.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Версия записи. Нумеруется с нуля.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Смещение предыдущей версии записи.
        /// </summary>
        public long PreviousOffset { get; set; }

        /// <summary>
        /// Поля записи.
        /// </summary>
        public RecordFieldCollection Fields
        {
            get { return _fields; }
        }

        public bool Deleted
        {
            get { return (( Status & RecordStatus.LogicallyDeleted ) != 0); }
            set
            {
                if ( value )
                {
                    Status |= RecordStatus.LogicallyDeleted;
                }
                else
                {
                    Status &= ~RecordStatus.LogicallyDeleted;
                }
            }
        }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Используется при сортировке записей.
        /// </summary>
        public string SortKey { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        public object UserData { get; set; }

        #endregion

        #region Private members

        private readonly RecordFieldCollection _fields
            = new RecordFieldCollection();

        //private static bool _SelectBySubField
        //    (
        //        RecordField field,
        //        Func<SubField,bool> predicate
        //    )
        //{
        //    return field.SubFields.Any(predicate);
        //}

        private static void Append3031
            (
                StringBuilder builder,
                string format,
                params object[] args
            )
        {
            builder.AppendFormat ( format, args );
            builder.Append ( "\x001F\x001E" );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получить текст поля до разделителей подполей
        /// первого повторения поля с указанной меткой.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <returns>Текст поля или <c>null</c>.</returns>
        public string FM
            (
                string tag
            )
        {
            return Fields
                .GetField(tag)
                .FirstOrDefault()
                .GetFieldText();
        }

        public string[] FMA ( string tag )
        {
            return Fields
                .GetField ( tag )
                .GetFieldText ();
        }

        public string FM(string tag, char code)
        {
            return Fields
                .GetField(tag)
                .GetSubField(code)
                .FirstOrDefault()
                .GetSubFieldText();
        }

        public string[] FMA ( string tag, char code )
        {
            return Fields
                .GetField ( tag )
                .GetSubField ( code )
                .GetSubFieldText ();
        }

        public string FR ( string format )
        {
            FieldReference fr = new FieldReference(format);
            return fr.FormatSingle ( this );
        }

        public string FR ( string pre, string format, string post )
        {
            string result = FR ( format );
            return string.IsNullOrEmpty ( result )
                ? string.Empty
                : ( pre + result + post );
        }

        public bool FRE ( string format )
        {
            return string.IsNullOrEmpty ( FR ( format ) );
        }

        public string[] FRA ( string format )
        {
            FieldReference fr = new FieldReference(format);
            return fr.Format ( this );
        }

        /// <summary>
        /// Кодирование записи в клиентское представление.
        /// </summary>
        /// <param name="record">Запись для кодирования.</param>
        /// <param name="mfn">MFN записи (м. б. несуществующий).</param>
        /// <param name="status">The status.</param>
        /// <param name="version">Версия записи (чаще всего 1).</param>
        /// <returns>
        /// Закодированная запись.
        /// </returns>
        public static string EncodeRecord
            (
                IrbisRecord record,
                int mfn,
                int status,
                int version
            )
        {
            StringBuilder result = new StringBuilder();

            Append3031 
                (
                    result,
                    "{0}#{1}",
                    mfn,
                    status
                );
            Append3031 
                (
                    result,
                    "0#{0}",
                    version
                );

            foreach (RecordField field in record.Fields)
            {
                RecordField._EncodeField
                    (
                        result,
                        field
                    );
            }

            return result.ToString();
        }

        public static IrbisRecord Parse 
            ( 
                string text,
                int skipLines
            )
        {
            string[] lines = text.Split ( '\x1F' );
            return Parse ( lines, skipLines );
        }

        public IrbisRecord MergeParse
            (
                string[] text,
                int skipLines
            )
        {
            if (skipLines > 0)
            {
                text = text.Skip(skipLines).ToArray();
            }

            if (text.Length > 2)
            {
                Regex regex = new Regex(@"^(-?\d+)\#(\d*)?");
                Match match = regex.Match(text[0]);
                Mfn = Math.Abs(int.Parse(match.Groups[1].Value));
                if (match.Groups[2].Length > 0)
                {
                    Status = (RecordStatus)int.Parse(match.Groups[2].Value);
                }
                match = regex.Match(text[1]);
                if (match.Groups[2].Length > 0)
                {
                    Version = int.Parse(match.Groups[2].Value);
                }
            }

            foreach (string line in text.Skip(2))
            {
                RecordField field = RecordField.Parse(line);
                if (field != null)
                {
                    Fields.Add(field);
                }
            }

            return this;
        }

        /// <summary>
        /// Разбор текстового представления записи.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="skipLines"></param>
        /// <returns></returns>
        public static IrbisRecord Parse
            (
                string[] text,
                int skipLines
            )
        {
            IrbisRecord result = new IrbisRecord();

            return result.MergeParse
                (
                    text,
                    skipLines
                );
        }

        /// <summary>
        /// Добавление поля (в конец списка полей) 
        /// с указанными меткой и текстом.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="text">Текст поля до разделителей.</param>
        /// <returns>this</returns>
        public IrbisRecord AddField
            (
                string tag,
                string text
            )
        {
            Fields.Add ( new RecordField(tag, text) );
            return this;
        }

        /// <summary>
        /// Добавление поля (в конец списка полей)
        /// с указанными меткой и подполями.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code">Код первого подполя.</param>
        /// <param name="text">Текст первого подполя.</param>
        /// <param name="others">Коды и тексты последующих
        /// подполей.</param>
        /// <returns>this</returns>
        public IrbisRecord AddField
            (
                string tag,
                char code,
                string text,
                params object[] others
            )
        {
            RecordField field = new RecordField(tag);
            field.AddSubField ( code, text );
            for ( int i = 0; i < others.Length; i+=2 )
            {
                char c = (char) others [ i ];
                string t = (string) others [ i + 1 ];
                field.AddSubField ( c, t );
            }
            Fields.Add ( field );
            return this;
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="text">The newText.</param>
        /// <returns></returns>
        /// <remarks>Устанавливает значение только для
        /// первого повторения поля (если в записи их несколько)!
        /// </remarks>
        public IrbisRecord SetField
            (
                string tag,
                string text
            )
        {
            RecordField field = Fields
                .GetField ( tag )
                .FirstOrDefault ();
            
            if ( field == null )
            {
                field = new RecordField(tag);
                Fields.Add ( field );
            }
            
            field.Text = text;
            
            return this;
        }

        public IrbisRecord SetField
            (
                string tag,
                int occurrence,
                string newText
            )
        {
            RecordField field = Fields
                .GetField(tag)
                .GetOccurrence(occurrence);
            
            if (!ReferenceEquals(field, null))
            {
                field.Text = newText;
            }

            return this;
        }

        public IrbisRecord SetSubField
            (
                string tag,
                char code,
                string text
            )
        {
            RecordField field = Fields
                .GetField ( tag )
                .FirstOrDefault ();

            if ( field == null )
            {
                field = new RecordField(tag);
                Fields.Add ( field );
            }

            field.SetSubField ( code, text );

            return this;
        }

        public IrbisRecord SetSubField
            (
                string tag,
                int fieldOccurrence,
                char code,
                int subFieldOccurrence,
                string newText
            )
        {
            RecordField field = Fields
                .GetField(tag)
                .GetOccurrence(fieldOccurrence);

            if (!ReferenceEquals(field, null))
            {
                SubField subField = field.GetSubField
                    (
                        code,
                        subFieldOccurrence
                    );
                if (!ReferenceEquals(subField, null))
                {
                    subField.Text = newText;
                }
            }

            return this;
        }

        /// <summary>
        /// Removes the field.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public IrbisRecord RemoveField
            (
                string tag
            )
        {
            RecordField[] found = Fields
                .GetField ( tag );
            foreach ( RecordField field in found )
            {
                Fields.Remove ( field );
            }
            return this;
        }

        public IrbisRecord RemoveField
            (
                string tag,
                int occurrence
            )
        {
            RecordField[] found = Fields.GetField(tag);
            if (occurrence < 0)
            {
                occurrence = found.Length + occurrence;
            }
            if (occurrence >= 0)
            {
                RecordField target = found.Skip(occurrence).FirstOrDefault();
                if (!ReferenceEquals(target, null))
                {
                    Fields.Remove(target);
                }
            }
            return this;
        }

        public bool HaveField
            (
                params string[] tags
            )
        {
            return (Fields.GetField(tags).Length != 0);
        }

        public bool HaveNotField
            (
                params string[] tags
            )
        {
            return (Fields.GetField(tags).Length == 0);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public IrbisRecord Clone ()
        {
            IrbisRecord result = new IrbisRecord ();

            result.Fields.AddRange 
                (
                    from field in Fields 
                    select field.Clone ()
                );

            return result;
        }

        public static int Compare
            (
                IrbisRecord record1,
                IrbisRecord record2,
                bool verbose
            )
        {
            IEnumerator<RecordField> enum1 = record1.Fields.GetEnumerator();
            IEnumerator<RecordField> enum2 = record2.Fields.GetEnumerator();

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

                RecordField field1 = enum1.Current;
                RecordField field2 = enum2.Current;

                int result = RecordField.Compare
                    (
                        field1,
                        field2,
                        verbose
                    );
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        public string ToPlainText()
        {
            StringBuilder result = new StringBuilder();

            foreach (RecordField field in Fields)
            {
                result.AppendFormat("{0}#", field.Tag);
                bool begin = true;
                if (!string.IsNullOrEmpty(field.Text))
                {
                    result.Append(field.Text);
                    begin = false;
                }
                foreach (SubField subField in field.SubFields)
                {
                    if (!begin)
                    {
                        result.Append(" ");
                    }
                    result.Append(subField.Text);
                    begin = false;
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        public string ToIsisText()
        {
            StringBuilder result = new StringBuilder();

            foreach (RecordField field in Fields)
            {
                result.AppendFormat
                    (
                        "<{0}>{1}</{0}>",
                        field.Tag,
                        field.ToText()
                    );
                result.AppendLine();
            }

            return result.ToString();
        }

        public static IrbisRecord ParseIsisRecord(string text)
        {
            throw new NotImplementedException("ParseIsisRecord not implemented");
        }

        #region Парсинг ISO2709

        const int MarkerLength = 24;
        const byte RecordDelimiter = 0x1D;
        const char FieldDelimiter = (char)0x1E;
        const char SubfieldDelimiter = (char)0x1F;

        private static int _ToInt(byte[] bytes, int offset, int count)
        {
            int result = 0;

            for (; count > 0; count--, offset++)
            {
                result = result * 10 + (bytes[offset] - ((byte)'0'));
            }
            return result;
        }

        public static IrbisRecord ReadIso
            (
                Stream strm,
                Encoding enc
            )
        {
            IrbisRecord result = new IrbisRecord();

            byte[] marker = new byte[5];

            // Считываем длину записи
            if (strm.Read(marker, 0, 5) != 5)
            {
                return null;
            }
            int reclen = _ToInt(marker, 0, 5);
            byte[] record = new byte[reclen];
            int need = reclen - 5;
            // А затем и ее остаток
            if (strm.Read(record, 5, need) != need)
            {
                return null;
            }

            // простая проверка, что мы имеем дело с нормальной ISO-записью
            if (record[reclen - 1] != RecordDelimiter)
            {
                return null;
            }

            // Превращаем в Unicode
            char[] chars = enc.GetChars(record, 0, reclen);
            int baseAddress = _ToInt(record, 12, 5) - 1;
            int start = baseAddress;

            // Пошли по полям (при помощи словаря)
            for (int dic = MarkerLength; ; dic += 12)
            {
                // находим следующее поле
                // Если нарвались на разделитель, заканчиваем
                if ((record[dic] == FieldDelimiter)
                     || (start > (reclen - 4)))
                {
                    break;
                }
                string tag = new string(chars, dic, 3);
                RecordField fld = new RecordField(tag);
                bool isFixed = tag.StartsWith("00");
                result.Fields.Add(fld);
                start++;
                int end;
                if (isFixed)
                {
                    for (end = start; ; end++)
                    {
                        if (record[end] == FieldDelimiter)
                        {
                            break;
                        }
                    }
                    fld.Text = new string(chars, start, end - start);
                    start = end;
                }
                else
                {
                    start += 2;
                    while (true)
                    {
                        // находим подполя
                        if (record[start] == FieldDelimiter)
                        {
                            break;
                        }
                        if (record[start] != SubfieldDelimiter)
                        {
                            // Нарвались на поле без подполей
                            for (end = start; ; end++)
                            {
                                if ((record[end] == FieldDelimiter)
                                     || (record[end] == SubfieldDelimiter))
                                {
                                    break;
                                }
                            }
                            fld.Text = new string
                                (chars,
                                  start,
                                  end - start);
                        }
                        else
                        {
                            // Декодируем подполя
                            SubField sub = new SubField
                                (chars[++start]);
                            fld.SubFields.Add(sub);
                            start++;
                            for (end = start; ; end++)
                            {
                                if ((record[end] == FieldDelimiter)
                                     || (record[end] == SubfieldDelimiter))
                                {
                                    break;
                                }
                            }
                            sub.Text = new string
                                (chars,
                                  start,
                                  end - start);
                        }
                        start = end;
                    }
                }
            }

            return result;
        }

        #endregion

        #region Парсинг текстового формата ИРБИС

        private static string _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                char c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString();
        }

        private static RecordField _ParseLine
            (
                string line
            )
        {
            StringReader reader = new StringReader(line);

            RecordField result = new RecordField
            {
                Tag = _ReadTo(reader, '#'),
                Text = _ReadTo(reader, '^')
            };

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                char code = char.ToLower((char)next);
                string text = _ReadTo(reader, '^');
                SubField subField = new SubField
                {
                    Code = code,
                    Text = text
                };
                result.SubFields.Add(subField);
            }

            return result;
        }

        /// <summary>
        /// Метод предназачен для парсинга текста, передаваемого АРМ ИРБИС плагинам.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IrbisRecord ParseIrbisText
            (
                string text
            )
        {
            string[] lines = text
                .Split('\n')
                .Select(line => line.Trim())
                .ToArray();

            string[] header = lines.Take(3).ToArray();
            StringReader reader = new StringReader(header[1]);
            int mfn = int.Parse(_ReadTo(reader, '#'));
            reader = new StringReader(header[2]);
            _ReadTo(reader, '#');

            // ReSharper disable once AssignNullToNotNullAttribute
            int version = int.Parse(reader.ReadToEnd());
            string[] body = lines.Skip(3).ToArray();

            IrbisRecord result = new IrbisRecord
            {
                Mfn = mfn,
                Version = version
            };
            result.Fields
                .AddRange
                (
                    body
                    .Select(line => _ParseLine(line))
                    .Where(item => item != null)
                );

            return result;
        }

        /// <summary>
        /// Метод предназначен для возврата значения из ИРБИС-плагинов.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static string EncodeIrbisText
            (
                IrbisRecord record
            )
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("0");
            result.AppendLine(record.Mfn + "#0");
            result.AppendLine("0#" + record.Version);

            foreach (RecordField field in record.Fields)
            {
                result.AppendFormat("{0}#", field.Tag);
                if (!string.IsNullOrEmpty(field.Text))
                {
                    result.Append(field.Text);
                }
                foreach (SubField subField in field.SubFields)
                {
                    result.AppendFormat("^{0}{1}", subField.Code, subField.Text);
                }
                result.AppendLine();
            }

            return result.ToString();
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
            return EncodeRecord
                (
                    this,
                    Mfn,
                    (int)Status,
                    Version
                );
        }

        #endregion
    }
}

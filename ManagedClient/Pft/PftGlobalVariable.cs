/* PftGlobalVariable.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Глобальная переменная в языке форматирования ИРБИС.
    /// Может содержать произвольное количество повторений поля.
    /// </summary>
    [Serializable]
    public sealed class PftGlobalVariable
    {
        #region Properties

        /// <summary>
        /// Номер глобальной переменной.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Поля.
        /// </summary>
        public List<RecordField> Fields { get { return _fields; } }

        #endregion

        #region Construction

        public PftGlobalVariable()
        {
            _fields = new List<RecordField>();
        }

        #endregion

        #region Private members

        private readonly List<RecordField> _fields;

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

        #endregion

        #region Public methods

        public PftGlobalVariable Parse
            (
                string text
            )
        {
            string[] lines = text.SplitLines();
            foreach (string line in lines)
            {
                ParseLine(line);
            }
            return this;
        }

        public PftGlobalVariable ParseLine
            (
                string line
            )
        {
            StringReader reader = new StringReader(line);
            RecordField field = new RecordField(Number.ToInvariantString());
            Fields.Add(field);
            field.Text = _ReadTo(reader, '^');
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
                field.SubFields.Add(subField);
            }
            return this;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            bool first = true;

            foreach (RecordField field in Fields)
            {
                if (!first)
                {
                    result.AppendLine();
                }
                first = false;
                result.Append(field);
            }

            return result.ToString();
        }

        #endregion
    }
}

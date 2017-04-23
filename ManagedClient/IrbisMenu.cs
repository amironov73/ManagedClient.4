// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisMenu.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Содержимое файла MNU.
    /// </summary>
    [Serializable]
    public sealed class IrbisMenu
    {
        #region Constants

        /// <summary>
        /// Признак конца меню.
        /// </summary>
        public const string Stop = "*****";

        #endregion

        #region Nested classes

        public enum Sort
        {
            None,
            ByCode,
            ByComment
        }

        /// <summary>
        /// Запись в меню. Соответствует паре строк.
        /// </summary>
        [Serializable]
        public sealed class Entry
        {
            #region Properties

            public string Code { get; set; }

            public string Comment { get; set; }

            #endregion

            #region Object members

            /// <summary>
            /// Returns a <see cref="System.String" /> 
            /// that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> 
            /// that represents this instance.</returns>
            public override string ToString()
            {
                return string.Format
                    (
                        "{0} - {1}",
                        Code,
                        Comment
                    );
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла (чисто для идентификации).
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        private Entry[] _entries;

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static string TrimCode
            (
                string code
            )
        {
            code = code.Trim();
            string[] parts = code.Split(' ', '-', '=', ':');
            if (parts.Length != 0)
            {
                code = parts[0];
            }
            return code;
        }

        public Entry FindEntry
            (
                string code
            )
        {
            return _entries
                .FirstOrDefault(entry => entry.Code.SameString(code));
        }

        public Entry FindEntrySensitive
            (
                string code
            )
        {
            return _entries
                .FirstOrDefault(entry => entry.Code.SameStringSensitive(code));
        }

        public Entry GetEntry
            (
                string code
            )
        {
            Entry candidate = FindEntry(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = code.Trim();
            candidate = FindEntry(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = TrimCode(code);
            candidate = FindEntry(code);
            if (candidate != null)
            {
                return candidate;
            }

            return null;
        }

        public Entry GetEntrySensitive
            (
                string code
            )
        {
            Entry candidate = FindEntrySensitive(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = code.Trim();
            candidate = FindEntrySensitive(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = TrimCode(code);
            candidate = FindEntrySensitive(code);
            if (candidate != null)
            {
                return candidate;
            }

            return null;
        }

        public string GetString
            (
                string code,
                string defaultValue
            )
        {
            Entry found = FindEntry(code);
            return (found == null)
                ? defaultValue
                : found.Comment;
        }

        public string GetStringSensitive
            (
                string code,
                string defaultValue
            )
        {
            Entry found = FindEntrySensitive(code);
            return (found == null)
                ? defaultValue
                : found.Comment;
        }

        public T Get<T>
            (
                string code,
                T defaultValue
            )
        {
            Entry found = FindEntry(code);
            return (found == null)
                ? defaultValue
                : (T)Convert.ChangeType(found.Comment, typeof(T));
        }

        public T GetSensitive<T>
            (
                string code,
                T defaultValue
            )
        {
            Entry found = FindEntrySensitive(code);
            return (found == null)
                ? defaultValue
                : (T)Convert.ChangeType(found.Comment, typeof(T));
        }

        public string[] CollectStrings
            (
                string code
            )
        {
            return _entries
                .Where(entry => entry.Code.SameString(code)
                                || TrimCode(entry.Code).SameString(code))
                .Select(entry => entry.Comment)
                .ToArray();
        }

        public static IrbisMenu Read
            (
                ManagedClient64 client,
                IrbisPath path,
                string name
            )
        {
            string text = client.ReadTextFile(path, name);
            return ParseText(text);
        }

        public static IrbisMenu Read
            (
                ManagedClient64 client,
                string name
            )
        {
            string text = client.ReadTextFile(name);
            return ParseText(text);
        }

        public static IrbisMenu ParseLines
            (
                string[] lines
            )
        {
            IrbisMenu result = new IrbisMenu();
            List<Entry> entries = new List<Entry>();

            int stop = lines.Length - 1;
            for (int i = 0; i < stop; i += 2)
            {
                string code = lines[i];
                string comment = lines[i + 1];

                if (code.StartsWith(Stop))
                {
                    break;
                }

                Entry entry = new Entry
                {
                    Code = code,
                    Comment = comment
                };
                entries.Add(entry);
            }

            result._entries = entries.ToArray();

            return result;
        }

        public static IrbisMenu ParseFile
            (
                string fileName,
                Encoding encoding
            )
        {
            string[] lines = File.ReadAllLines
                (
                    fileName,
                    encoding
                );

            IrbisMenu result = ParseLines(lines);
            result.Name = fileName;

            return result;
        }

        public static IrbisMenu ParseText
            (
                string text
            )
        {
            string[] lines = text.SplitLines();
            return ParseLines(lines);
        }

        public Entry[] SortEntries(Sort sortBy)
        {
            List<Entry> copy = new List<Entry>(_entries);
            switch (sortBy)
            {
                case Sort.ByCode:
                    copy = copy.OrderBy(entry => entry.Code).ToList();
                    break;
                case Sort.ByComment:
                    copy = copy.OrderBy(entry => entry.Comment).ToList();
                    break;
            }

            return copy.ToArray();
        }

        public string ToText()
        {
            StringBuilder result = new StringBuilder();

            foreach (Entry entry in _entries)
            {
                result.AppendLine(entry.Code);
                result.AppendLine(entry.Comment);
            }
            result.AppendLine(Stop);

            return result.ToString();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (Entry entry in _entries)
            {
                result.AppendLine(entry.ToString());
            }
            return result.ToString();
        }

        #endregion
    }
}

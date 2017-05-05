// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTree.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Дерево (иерархический справочник).
    /// </summary>
    [Serializable]
    public sealed class IrbisTree
    {
        #region Constants

        /// <summary>
        /// Tabulation
        /// </summary>
        public const char Indent = '\x09';

        #endregion

        #region Nested classes

        /// <summary>
        /// Tree item
        /// </summary>
        [PublicAPI]
        [MoonSharpUserData]

        public sealed class Item
        {
            #region Properties

            /// <summary>
            /// Children.
            /// </summary>
            [NotNull]
            [ItemNotNull]
            public List<Item> Children
            {
                get { return _children; }
            }

            /// <summary>
            /// Delimiter.
            /// </summary>
            public static string Delimiter
            {
                get { return _delimiter; }
                set { SetDelimiter(value); }
            }

            /// <summary>
            /// Prefix.
            /// </summary>
            public string Prefix { get { return _prefix; } }

            /// <summary>
            /// Suffix.
            /// </summary>
            public string Suffix { get { return _suffix; } }

            /// <summary>
            /// Value.
            /// </summary>
            [CanBeNull]

            public string Value
            {
                get { return _value; }
                set
                {
                    SetValue(value);
                }
            }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item()
            {
                _children = new List<Item>();
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item
                (
                    [CanBeNull] string value
                )
                : this()
            {
                SetValue(value);
            }

            #endregion

            #region Private members

            private readonly List<Item> _children;

            private static string _delimiter = " - ";

            private string _prefix, _suffix, _value;

            internal int _level;

            #endregion

            #region Public methods

            /// <summary>
            /// Add child.
            /// </summary>
            [NotNull]
            public Item AddChild
                (
                    [CanBeNull] string value
                )
            {
                Item result = new Item(value);
                Children.Add(result);

                return result;
            }

            /// <summary>
            /// Set the delimiter.
            /// </summary>
            public static void SetDelimiter
                (
                    [CanBeNull] string value
                )
            {
                _delimiter = value;
            }

            /// <summary>
            /// Set the value.
            /// </summary>
            public void SetValue
                (
                    [CanBeNull] string value
                )
            {
                _value = value;
                _prefix = null;
                _suffix = null;

                if (!string.IsNullOrEmpty(Delimiter)
                    && !string.IsNullOrEmpty(value))
                {

                    string[] parts = value.Split
                        (
                            new[] { Delimiter },
                            2,
                            StringSplitOptions.None
                        );

                    _prefix = parts[0];
                    if (parts.Length != 1)
                    {
                        _suffix = parts[1];
                    }
                }
            }

            /// <summary>
            /// Verify the item.
            /// </summary>
            public bool Verify
                (
                    bool throwException
                )
            {
                bool result = !string.IsNullOrEmpty(Value);

                if (result &&
                    (Children.Count != 0))
                {
                    result = Children.All
                        (
                            child => child.Verify(throwException)
                        );
                }

                if (!result && throwException)
                {
                    throw new FormatException();
                }

                return result;
            }

            #endregion
        }


        #endregion

        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        /// <summary>
        /// Root items.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public List<Item> Roots
        {
            get { return _roots; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTree()
        {
            _roots = new List<Item>();
        }

        #endregion

        #region Private members

        private readonly List<Item> _roots;

        /// <summary>
        /// Determines indent level of the string.
        /// </summary>
        private static int CountIndent
            (
                [NotNull] string line
            )
        {
            int result = 0;

            foreach (char c in line)
            {
                if (c == Indent)
                {
                    result++;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private static int _ArrangeLevel
            (
                List<Item> items,
                int level,
                int index,
                int count
            )
        {
            int next = index + 1;
            int level2 = level + 1;

            while (next < count)
            {
                if (items[next]._level <= level)
                {
                    break;
                }

                if (items[next]._level == level2)
                {
                    items[index].Children.Add(items[next]);
                }

                next++;
            }

            return next;
        }

        private static void _ArrangeLevel
            (
                List<Item> items,
                int level
            )
        {
            int count = items.Count;
            int index = 0;

            while (index < count)
            {
                int next = _ArrangeLevel
                    (
                        items,
                        level,
                        index,
                        count
                    );

                index = next;
            }
        }

        private static void _WriteLevel
            (
                TextWriter writer,
                List<Item> items,
                int level
            )
        {
            foreach (Item item in items)
            {
                for (int i = 0; i < level; i++)
                {
                    writer.Write(Indent);
                }
                writer.WriteLine(item.Value);

                _WriteLevel
                    (
                        writer,
                        item.Children,
                        level + 1
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add root item.
        /// </summary>
        [NotNull]
        public Item AddRoot
            (
                [CanBeNull] string value
            )
        {
            Item result = new Item(value);
            Roots.Add(result);

            return result;
        }

        /// <summary>
        /// Parse specified stream.
        /// </summary>
        [NotNull]
        public static IrbisTree ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            IrbisTree result = new IrbisTree();

            List<Item> list = new List<Item>();
            string line = reader.ReadLine();
            if (line == null)
            {
                goto DONE;
            }
            if (CountIndent(line) != 0)
            {
                throw new FormatException();
            }
            list.Add(new Item(line));

            int currentLevel = 0;
            while ((line = reader.ReadLine()) != null)
            {
                int level = CountIndent(line);
                if (level > (currentLevel + 1))
                {
                    throw new FormatException();
                }
                currentLevel = level;
                line = line.TrimStart(Indent);
                Item item = new Item(line)
                {
                    _level = currentLevel
                };
                list.Add(item);
            }

            int maxLevel = list.Max(item => item._level);
            for (int level = 0; level < maxLevel; level++)
            {
                _ArrangeLevel(list, level);
            }

            var roots = list.Where(item => item._level == 0);
            result.Roots.AddRange(roots);

            DONE:
            return result;
        }

        /// <summary>
        /// Read local file.
        /// </summary>
        [NotNull]
        public static IrbisTree ReadLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            using (StreamReader reader
                = new StreamReader
                    (
                        File.OpenRead(fileName),
                        encoding
                    ))
            {
                IrbisTree result = ParseStream(reader);
                result.FileName = Path.GetFileName(fileName);

                return result;
            }
        }

        /// <summary>
        /// Save to text stream.
        /// </summary>
        public void Save
            (
                [NotNull] TextWriter writer
            )
        {
            _WriteLevel
                (
                    writer,
                    Roots,
                    0
                );
        }

        /// <summary>
        /// Save to local file.
        /// </summary>
        public void SaveToLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            using (StreamWriter writer = new StreamWriter
                    (
                        File.Create(fileName),
                        encoding
                    ))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Verify the tree.
        /// </summary>
        public bool Verify
            (
                bool throwException
            )
        {
            bool result = (Roots.Count != 0)
                && Roots.All
                    (
                        root => root.Verify(throwException)
                    );

            if (!result && throwException)
            {
                throw new FormatException();
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return string.Format
                (
                    "FileName: {0}", 
                    FileName
                );
        }

        #endregion
    }
}

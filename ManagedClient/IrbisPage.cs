// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisPage.cs -- worksheet for RecordField editing
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Worksheet for RecordField editing
    /// </summary>
    [Serializable]
    public sealed class IrbisPage
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Lines.
        /// </summary>
        [NotNull]
        public List<IrbisLine> Lines { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisPage"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public IrbisPage
            (
                [NotNull] string name
            )
        {
            Name = name;
            Lines = new List<IrbisLine>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the lines.
        /// </summary>
        public void ParseLines
            (
                [NotNull] string[] lines
            )
        {
            int lineCount = int.Parse(lines[0]);
            int skip = 1;
            for (int index = 0; index < lineCount; index++)
            {
                string[] portion = lines.Skip(skip).Take(10).ToArray();
                IrbisLine item = new IrbisLine();
                item.ParseLines(portion);
                Lines.Add(item);
                skip += 10;
            }
        }

        /// <summary>
        /// Parse the text.
        /// </summary>
        public void ParseText
            (
                [NotNull] string text
            )
        {
            string[] lines = text.SplitLines();
            ParseLines(lines);
        }

        /// <summary>
        /// Parse the file.
        /// </summary>
        public void ParseFile
            (
                [NotNull] string fileName
            )
        {
            Name = Path.GetFileNameWithoutExtension(fileName);
            string[] lines = File.ReadAllLines
                (
                    fileName,
                    Encoding.Default
                );
            ParseLines(lines);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(Name);
            foreach (IrbisLine line in Lines)
            {
                result.AppendLine(line.ToString());
            }

            return result.ToString();
        }

        #endregion
    }
}

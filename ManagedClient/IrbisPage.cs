/* IrbisPage.cs -- worksheet for RecordField editing
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
    /// Worksheet for RecordField editing
    /// </summary>
    [Serializable]
    public sealed class IrbisPage
    {
        #region Properties

        public string Name { get; private set; }

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
                string name
            )
        {
            Name = name;
            Lines = new List<IrbisLine>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public void ParseLines
            (
                string[] lines
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

        public void ParseText
            (
                string text
            )
        {
            string[] lines = text.SplitLines();
            ParseLines(lines);
        }

        public void ParseFile
            (
                string fileName
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

        /// <summary>
        /// Returns a <see cref="System.String" /> 
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> 
        /// that represents this instance.</returns>
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

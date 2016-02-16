/* IrbisWorkSheet.cs -- wrapper for WS file
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
    /// Wrapper for WS file
    /// </summary>
    [Serializable]
    public sealed class IrbisWorkSheet
    {
        #region Properties

        /// <summary>
        /// Имя рабочего листа, например, PAZK.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// Страницы рабочего листа.
        /// </summary>
        /// <value></value>
        public List<IrbisPage> Pages { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisWorkSheet"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public IrbisWorkSheet
            (
                string name
            )
        {
            Name = name;
            Pages = new List<IrbisPage>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Парсинг строк.
        /// </summary>
        /// <param name="lines"></param>
        public void ParseLines
            (
                string[] lines
            )
        {
            int pageCount = int.Parse(lines[0]);
            for (int i = 0; i < pageCount; i++)
            {
                IrbisPage page = new IrbisPage(lines[i + 1]);
                Pages.Add(page);
            }
            int skip = 1 + pageCount * 2;
            for (int index = 0; index < pageCount; index++)
            {
                IrbisPage page = Pages[index];
                int lineCount = int.Parse(lines[1 + pageCount + index]);
                for (int j = 0; j < lineCount; j++)
                {
                    IrbisLine item = new IrbisLine();
                    string[] portion = lines.Skip(skip).Take(10).ToArray();
                    item.ParseLines(portion);
                    page.Lines.Add(item);
                    skip += 10;
                }
            }
        }

        /// <summary>
        /// Парсинг текста.
        /// </summary>
        /// <param name="text"></param>
        public void ParseText
            (
                string text
            )
        {
            string[] lines = text.SplitLines();
            ParseLines(lines);
        }

        /// <summary>
        /// Парсинг локального файла.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(Name);
            foreach (IrbisPage page in Pages)
            {
                result.AppendLine(page.ToString());
            }
            return result.ToString();
        }

        #endregion
    }
}

/* IrbisStopWords.cs -- wrapper for STW file
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
    /// Wrapper for STW file
    /// </summary>
    [Serializable]
    public sealed class IrbisStopWords
    {
        #region Properties

        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisStopWords"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public IrbisStopWords
            (
                string name
            )
        {
            Name = name;
            _dictionary = new Dictionary<string, object>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, object> _dictionary;

        #endregion

        #region Public methods

        public bool IsStopWord
            (
                string word
            )
        {
            if (string.IsNullOrEmpty(word))
            {
                return true;
            }
            word = word.Trim();
            if (string.IsNullOrEmpty(word))
            {
                return true;
            }
            return _dictionary.ContainsKey(word);
        }

        public static IrbisStopWords ParseLines
            (
                string name,
                string[] lines
            )
        {
            IrbisStopWords result = new IrbisStopWords(name);

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    result._dictionary[trimmed] = null;
                }
            }

            return result;
        }

        public static IrbisStopWords ParseText
            (
                string name,
                string text
            )
        {
            string[] lines = text.SplitLines();

            return ParseLines
                (
                    name,
                    lines
                );
        }

        public static IrbisStopWords ParseFile
            (
                string fileName
            )
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string[] lines = File.ReadAllLines
                (
                    fileName,
                    Encoding.Default
                );
            return ParseLines
                (
                    name,
                    lines
                );
        }

        public string[] ToLines()
        {
            return _dictionary
                .Keys
                .OrderBy(word => word)
                .ToArray();
        }

        public string ToText()
        {
            return string.Join
                (
                    Environment.NewLine,
                    ToLines()
                );
        }

        #endregion
    }
}

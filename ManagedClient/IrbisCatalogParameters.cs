/* IrbisCatalogParameters.cs -- wrapper for PAR file
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Wrapper for PAR file.
    /// </summary>
    [Serializable]
    public sealed class IrbisCatalogParameters
    {
        #region Properties

        /// <summary>
        /// Имя каталога.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Параметр №1
        /// </summary>
        public string Xrf { get; set; }

        /// <summary>
        /// Параметр №2
        /// </summary>
        public string Mst { get; set; }

        /// <summary>
        /// Параметр №3
        /// </summary>
        public string Cnt { get; set; }

        /// <summary>
        /// Параметр №4
        /// </summary>
        public string N01 { get; set; }

        /// <summary>
        /// Параметр №5
        /// </summary>
        public string N02 { get; set; }

        /// <summary>
        /// Параметр №6
        /// </summary>
        public string L01 { get; set; }

        /// <summary>
        /// Параметр №7
        /// </summary>
        public string L02 { get; set; }

        /// <summary>
        /// Параметр №8
        /// </summary>
        public string Ifp { get; set; }

        /// <summary>
        /// Параметр №9
        /// </summary>
        public string Any { get; set; }

        /// <summary>
        /// Параметр №10
        /// </summary>
        public string Pft { get; set; }

        /// <summary>
        /// Параметр №11
        /// </summary>       
        public string External { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisCatalogParameters"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public IrbisCatalogParameters
            (
                string name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Name = name;

            string defaultPath = string.Format
                (
                    @".\datai\{0}",
                    name
                );
            Xrf = defaultPath;
            Mst = defaultPath;
            Cnt = defaultPath;
            N01 = defaultPath;
            N02 = defaultPath;
            L01 = defaultPath;
            L02 = defaultPath;
            Ifp = defaultPath;
            Any = defaultPath;
            Pft = defaultPath;
            External = defaultPath;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static IrbisCatalogParameters ParseLines
            (
                string name,
                string[] lines
            )
        {
            Regex regex = new Regex
                (
                    @"^(?<number>\d+)=(?<value>.+)$", 
                    RegexOptions.Multiline
                );
            
            IrbisCatalogParameters result = new IrbisCatalogParameters(name);
            
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                string trimmed = line.Trim();
                Match match = regex.Match(trimmed);
                if (match.Success)
                {
                    int number = int.Parse(match.Groups["number"].Value);
                    string value = match.Groups["value"].Value.Trim();

                    switch (number)
                    {
                        case 1:
                            result.Xrf = value;
                            break;
                        case 2:
                            result.Mst = value;
                            break;
                        case 3:
                            result.Cnt = value;
                            break;
                        case 4:
                            result.N01 = value;
                            break;
                        case 5:
                            result.N02 = value;
                            break;
                        case 6:
                            result.L01 = value;
                            break;
                        case 7:
                            result.L02 = value;
                            break;
                        case 8:
                            result.Ifp = value;
                            break;
                        case 9:
                            result.Any = value;
                            break;
                        case 10:
                            result.Pft = value;
                            break;
                        case 11:
                            result.External = value;
                            break;
                    }
                }
            }

            return result;
        }

        public static IrbisCatalogParameters ParseText
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

        public static IrbisCatalogParameters ParseFile
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

        #endregion

        #region Object members

        #endregion
    }
}

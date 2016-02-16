/* SearchPostingInfo.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

namespace ManagedClient
{
    [Serializable]
    public sealed class SearchPostingInfo
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Номер повторения поля
        /// </summary>
        public int Occurences { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Текст постинга.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Если было запрошено форматирование.
        /// </summary>
        public string Formatted { get; set; }

        #endregion

        #region Public methods

        public static SearchPostingInfo[] Parse
            (
                IEnumerable < string > answer
            )
        {
            List<SearchPostingInfo> result = new List<SearchPostingInfo>();

            Regex regex = new Regex(@"^(\d+)\#(\w+)\#(\d+)\#(\d+)\#(\d*)$");
            foreach (string line in answer)
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    SearchPostingInfo item = new SearchPostingInfo
                    {
                        Mfn = int.Parse(match.Groups[1].Value),
                        Tag = match.Groups[2].Value,
                        Occurences = int.Parse(match.Groups[2].Value),
                        Count = int.Parse(match.Groups[3].Value),
                    };
                    result.Add(item);
                }
            }

            return result.ToArray ();
        }

        public static SearchPostingInfo[] ParseFormatted
            (
                IEnumerable<string> answer
            )
        {
            List<SearchPostingInfo> result = new List<SearchPostingInfo>();

            Regex regex = new Regex(@"^(\d+)\#(\d+)\#(\w+)\#(\d+)\#(\d+)$");
            foreach (string line in answer)
            {
                string[] parts = line.Split('\x1E');
                if (parts.Length != 3)
                {
                    continue;
                }

                Match match = regex.Match(parts[0]);
                if (match.Success)
                {
                    SearchPostingInfo item = new SearchPostingInfo
                    {
                        Mfn = int.Parse(match.Groups[2].Value),
                        Tag = match.Groups[3].Value,
                        Occurences = int.Parse(match.Groups[4].Value),
                        Count = int.Parse(match.Groups[5].Value),
                        Text = parts[1],
                        Formatted = parts[2]
                    };
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "{0}#{1}#{2}#{3}", 
                    Mfn, 
                    Tag, 
                    Occurences, 
                    Count 
                );
        }

        #endregion
    }
}

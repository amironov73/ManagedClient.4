// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchTermInfo.cs -- информация о поисковом термине
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Информация о поисковом термине.
    /// </summary>
    [Serializable]
    [XmlRoot("term")]
    [MoonSharpUserData]
    public sealed class SearchTermInfo
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        [XmlAttribute("count")]
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        [XmlAttribute("text")]
        [JsonProperty("text")]
        public string Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера
        /// </summary>
        public static SearchTermInfo[] Parse
            (
                IEnumerable<string> answer
            )
        {
            List<SearchTermInfo> result = new List<SearchTermInfo>();

            Regex regex = new Regex(@"^(\d+)\#(.+)$");
            foreach (string line in answer)
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    SearchTermInfo item = new SearchTermInfo
                        {
                            Count = int.Parse(match.Groups[1].Value),
                            Text = match.Groups[2].Value
                        };
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "{0}#{1}",
                    Count,
                    Text
                );
        }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchPostingInfo.cs -- информация о постинге
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
    /// Информация о постинге.
    /// </summary>
    [Serializable]
    [XmlRoot("posting")]
    [MoonSharpUserData]
    public sealed class SearchPostingInfo
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Номер повторения поля
        /// </summary>
        [XmlAttribute("occurrence")]
        [JsonProperty("occurrence")]
        public int Occurrence { get; set; }

        /// <summary>
        /// Количество повторений.
        /// </summary>
        [XmlAttribute("count")]
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Текст постинга.
        /// </summary>
        [XmlAttribute("text")]
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Если было запрошено форматирование.
        /// </summary>
        [XmlAttribute("formatted")]
        [JsonProperty("formatted")]
        public string Formatted { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public static SearchPostingInfo[] Parse
            (
                IEnumerable<string> answer
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
                        Occurrence = int.Parse(match.Groups[2].Value),
                        Count = int.Parse(match.Groups[3].Value),
                    };
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор ответа сервера с расформатированными записями.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
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
                        Occurrence = int.Parse(match.Groups[4].Value),
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
        public override string ToString()
        {
            return string.Format
                (
                    "{0}#{1}#{2}#{3}",
                    Mfn,
                    Tag,
                    Occurrence,
                    Count
                );
        }

        #endregion
    }
}

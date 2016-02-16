/* ChairInfo.cs -- кафедра обслуживания.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Readers
{
    /// <summary>
    /// Информация о кафедре обслуживания.
    /// </summary>
    [Serializable]
    [XmlRoot("chair")]
    public sealed class ChairInfo
    {
        #region Constants

        /// <summary>
        /// Имя меню с кафедрами по умолчанию.
        /// </summary>
        public const string ChairMenu = "kv.mnu";

        #endregion

        #region Properties

        /// <summary>
        /// Код.
        /// </summary>
        [XmlAttribute("code")]
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ChairInfo()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="code">Код.</param>
        /// <param name="title">Название.</param>
        public ChairInfo(string code, string title)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            Code = code;
            Title = title;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текста меню-файла.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ChairInfo[] Parse
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            List<ChairInfo> result = new List<ChairInfo>();

            text = text.Replace("\r", string.Empty);
            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i+=2)
            {
                if (lines[i].StartsWith("*"))
                {
                    break;
                }
                ChairInfo item = new ChairInfo
                    {
                        Code = lines[i],
                        Title = lines[i+1]
                    };
                result.Add(item);
            }

            result.Add
                (
                    new ChairInfo
                        {
                            Code = "*",
                            Title = "Все подразделения"
                        }
                );

            return result
                .OrderBy( item => item.Code )
                .ToArray();
        }

        /// <summary>
        /// Загрузка с сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ChairInfo[] Read
            (
                ManagedClient64 client,
                string fileName
            )
        {
            string chairText = client.ReadTextFile
                (
                    IrbisPath.MasterFile, 
                    fileName
                );

            ChairInfo[] result = Parse(chairText);

            return result;
        }

        /// <summary>
        /// Загрузка с сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static ChairInfo[] Read
            (
                ManagedClient64 client
            )
        {
            return Read
                (
                    client,
                    ChairMenu
                );
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "{0} - {1}", 
                    Code, 
                    Title
                );
        }

        #endregion
    }
}

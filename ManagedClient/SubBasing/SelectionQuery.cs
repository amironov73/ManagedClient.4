// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SelectionQuery.cs -- запрос на отбор записей.
 */

#region Using directives

using System;
using System.Text;
using System.Xml.Serialization;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.SubBasing
{
    /// <summary>
    /// Запрос на отбор записей.
    /// </summary>
    [Serializable]
    [XmlRoot("selection-query")]
    public sealed class SelectionQuery
    {
        #region Properties

        /// <summary>
        /// Описание в произвольной форме.
        /// Например, "Отбор по MFN".
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// Если не задано,
        /// используется текущая база данных.
        /// </summary>
        [XmlAttribute("database")]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// Тип отбора записей.
        /// </summary>
        [XmlAttribute("selection-type")]
        [JsonProperty("selection-type")]
        public SelectionType SelectionType { get; set; }

        /// <summary>
        /// Префикс.
        /// </summary>
        [XmlAttribute("prefix")]
        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        /// <summary>
        /// Формулировка запроса.
        /// Например, "1-100,150-160"
        /// </summary>
        [XmlAttribute("statement")]
        [JsonProperty("statement")]
        public string Statement { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Корректно заполнено?
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Statement))
            {
                return false;
            }

            if (SelectionType == SelectionType.Mfn)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    return false;
                }
            }
            else if (SelectionType == SelectionType.Sequential)
            {
                if (string.IsNullOrEmpty(Prefix))
                {
                    return false;
                }
            }
            else if (SelectionType == SelectionType.Search)
            {
                // Nothing to do?
            }
            else if (SelectionType == SelectionType.Deep)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Получение текстового описания.
        /// </summary>
        /// <returns></returns>
        public string ToText()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(Description))
            {
                result.AppendFormat("Description: {0}", Description);
                result.AppendLine();
            }

            if (!string.IsNullOrEmpty(Database))
            {
                result.AppendFormat("Database: {0}", Database);
                result.AppendLine();
            }

            result.AppendFormat("SelectionType: {0}", SelectionType);
            result.AppendLine();

            if (!string.IsNullOrEmpty(Prefix))
            {
                result.AppendFormat("Prefix: {0}", Prefix);
                result.AppendLine();
            }

            if (!string.IsNullOrEmpty(Statement))
            {
                result.AppendFormat("Statement: {0}", Statement);
                result.AppendLine();
            }

            return result.ToString();
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}

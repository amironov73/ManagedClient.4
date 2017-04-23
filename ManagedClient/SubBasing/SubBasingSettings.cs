// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubBasingSettings.cs -- варианты для отбора записей.
 */

#region Using directives

using System;
using System.Xml.Serialization;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.SubBasing
{
    /// <summary>
    /// Варианты для отбора записей.
    /// </summary>
    [Serializable]
    [XmlRoot("sub-base")]
    public sealed class SubBasingSettings
    {
        #region Properties

        /// <summary>
        /// Перечень баз данных.
        /// </summary>
        [XmlElement("database")]
        [JsonProperty("database")]
        public IrbisDatabaseInfo[] Databases { get; set; }

        /// <summary>
        /// База данных по умолчанию.
        /// </summary>
        [XmlElement("database-index")]
        [JsonProperty("database-index")]
        public int DatabaseIndex { get; set; }

        /// <summary>
        /// Критерии отбора.
        /// </summary>
        [XmlElement("criteria")]
        [JsonProperty("criteria")]
        public SelectionQuery[] Criteria { get; set; }


        /// <summary>
        /// Предлагаемый критерий отбора.
        /// </summary>
        [XmlElement("criteria-index")]
        [JsonProperty("criteria-index")]
        public int CriteriaIndex { get; set; }

        /// <summary>
        /// Предлагаемое выражение для отбора.
        /// </summary>
        [XmlElement("statement")]
        [JsonProperty("statement")]
        public string Statement { get; set; }

        #endregion
    }
}

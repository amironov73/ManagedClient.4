// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordReport.cs -- отчёт о проверке записи.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Отчёт о проверке записи.
    /// </summary>
    [Serializable]
    public sealed class RecordReport
    {
        #region Properties

        /// <summary>
        /// MFN записи.
        /// </summary>
        [JsonProperty("mfn")]
        [XmlAttribute("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи.
        /// </summary>
        [JsonProperty("index")]
        [XmlAttribute("index")]
        public string Index { get; set; }

        /// <summary>
        /// Краткое БО.
        /// </summary>
        [JsonProperty("description")]
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// Дефекты.
        /// </summary>
        [JsonProperty("defects")]
        [XmlArray("defects")]
        public List<FieldDefect> Defects { get; set; }

        /// <summary>
        /// Формальная оценка качества.
        /// </summary>
        [JsonProperty("gold")]
        [XmlAttribute("gold")]
        public int Gold { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RecordReport()
        {
            Defects = new List<FieldDefect>();
        }

        #endregion
    }
}

/* RecordReference.cs
 */

#region Using directives

using System;
using System.Xml.Serialization;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Ссылка на запись (например, для сохранения в "кармане").
    /// </summary>
    [Serializable]
    [XmlRoot("record")]
    public sealed class RecordReference
    {
        #region Properties

        /// <summary>
        /// Сервер ИРБИС64. Например, "127.0.0.1".
        /// </summary>
        [XmlAttribute("host")]
        public string HostName { get; set; }

        /// <summary>
        /// База данных. Например, "IBIS".
        /// </summary>
        [XmlAttribute("db")]
        public string Database { get; set; }

        /// <summary>
        /// MFN. Чаще всего = 0, т. к. используется Index.
        /// </summary>
        [XmlAttribute("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи в базе данных, например "81.432.1-42/P41-012833".
        /// </summary>
        [XmlAttribute("index")]
        public string Index { get; set; }

        #endregion
    }
}

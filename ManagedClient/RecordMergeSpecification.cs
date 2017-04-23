// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordMergeSpecification.cs
 */

#region Using directives

using System;
using System.Xml.Serialization;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Спецификация для слияния записей.
    /// </summary>
    [Serializable]
    [XmlRoot("merge")]
    public class RecordMergeSpecification
    {
        #region Properties

        /// <summary>
        /// Действие по умолчанию.
        /// </summary>
        [XmlElement ("default-action")]
        public MergeAction DefaultAction;

        /// <summary>
        /// Спецификации для слияния полей.
        /// </summary>
        [XmlElement("field")]
        public FieldMergeSpecification[] Fields;

        #endregion
    }
}

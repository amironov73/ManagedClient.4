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

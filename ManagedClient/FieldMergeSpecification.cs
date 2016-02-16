using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ManagedClient
{
    [Serializable]
    [XmlRoot("field")]
    public class FieldMergeSpecification
    {
        public string Tag { get; set; }

        public MergeAction Action { get; set; }
    }
}

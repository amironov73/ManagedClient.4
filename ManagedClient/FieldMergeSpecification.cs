// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

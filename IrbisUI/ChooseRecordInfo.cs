using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedClient;

namespace IrbisUI
{
    public sealed class ChooseRecordInfo
    {
        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public IrbisRecord Record { get; set; }

        public override string ToString()
        {
            return ShortDescription;
        }
    }
}

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace Tests
{
    [TestClass]
    public class IniFileTest
    {
        [TestMethod]
        public void TestIniFile1()
        {
            IniFile ini = new IniFile();
            IniFile.Section section = ini.GetOrCreateSection("Main");
            section.Add("Format", "@sbrief");
            section.Add("Prefix", "IN=");
            StringWriter writer = new StringWriter();
            ini.Save(writer);
            string text = writer.ToString();
            Assert.IsTrue(text.Length > 0);
        }
    }
}

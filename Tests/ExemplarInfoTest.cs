/* ExemplarInfoTest.cs
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using ManagedClient.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

#endregion

namespace Tests
{
    [TestClass]
    public class ExemplarInfoTest
    {
        [TestMethod]
        public void TestExemplarInfo1()
        {
            List<ExemplarInfo> exemplars;
            using (ManagedClient64 client = new ManagedClient64())
            {
                client.ParseConnectionString("host=192.168.3.2;port=6666;"
                    + "user=Никто;password=Нигде;db=IBIS;");
                client.Connect();
                int[] mfnList = Enumerable.Range(1, 6000).ToArray();
                BatchRecordReader reader = new BatchRecordReader(client, mfnList);
                List<IrbisRecord> records = reader.ReadAll();
                exemplars = records.SelectMany(ExemplarInfo.Parse).ToList();
            }
            //exemplars.ToArray().SaveToFile("exemplars.bin");
            exemplars.ToArray().SaveToZipFile("exemplars.biz");
        }

        [TestMethod]
        public void TestExemplarInfo2()
        {
            //ExemplarInfo[] exemplars = IrbisIOUtils.ReadFromFile
            //    (
            //        "exemplars.bin",
            //        ExemplarInfo.ReadFromStream
            //    );
            ExemplarInfo[] exemplars = IrbisIOUtils.ReadFromZipFile<ExemplarInfo>
                (
                    "exemplars.biz"
                );
            Assert.IsTrue(exemplars.Length > 0);
        }
    }
}

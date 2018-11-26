using System.Linq;

using ManagedClient;
using ManagedClient.Readers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    //[TestClass]
    public class ReaderInfoTest
    {
        //[TestMethod]
        public void TestReaderInfo()
        {
            const string fileName = "readers.biz";

            int[] mfnList = Enumerable.Range(1, 6000).ToArray();
            IrbisRecord[] records;
            using (BatchRecordReader reader = new BatchRecordReader
                (
                    "host=192.168.3.2;port=6666;user=Никто;" +
                    "password=Нигде;db=RDR;",
                    mfnList
                ))
            {
                records = reader.ReadAll().ToArray();
            }
            Assert.AreEqual(mfnList.Length, records.Length);

            ReaderInfo[] readers1 = records
                .Select(ReaderInfo.Parse)
                .ToArray();
            Assert.AreEqual(records.Length, readers1.Length);

            ReaderInfo.SaveToFile(fileName, readers1);

            ReaderInfo[] readers2 = ReaderInfo.ReadFromFile(fileName);
            Assert.IsNotNull(readers2);
            Assert.AreEqual(readers1.Length, readers2.Length);

            for (int i = 0; i < readers1.Length; i++)
            {
                Assert.AreEqual
                    (
                        readers1[i].Ticket,
                        readers2[i].Ticket
                    );
            }
        }
    }
}

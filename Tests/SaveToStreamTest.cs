using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace Tests
{
    [TestClass]
    public class SaveToStreamTest
    {
        [TestMethod]
        public void TestSaveToStream()
        {
            const string fileName = "records.biz";

            int[] mfnList = Enumerable.Range(1, 6000).ToArray();
            IrbisRecord[] written;
            using (BatchRecordReader reader = new BatchRecordReader
                (
                    "host=192.168.3.2;port=6666;user=Никто;" +
                    "password=Нигде;db=IBIS;",
                    mfnList
                ))
            {
                written = reader.ReadAll().ToArray();
            }
            Assert.AreEqual(mfnList.Length, written.Length);
            IrbisRecord.SaveToFile(fileName, written);

            IrbisRecord[] readed = IrbisRecord.ReadFromFile(fileName);
            Assert.AreEqual(written.Length, readed.Length);
        }
    }
}

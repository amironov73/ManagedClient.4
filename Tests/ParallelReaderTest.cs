/* ParallelReaderTest.cs
 */

#region Using directives

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

#endregion

namespace Tests
{
    [TestClass]
    public class ParallelReaderTest
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    int[] mfnList = Enumerable.Range(1, 6000).ToArray();
        //    ParallelRecordReader reader = new ParallelRecordReader
        //        (
        //            3,
        //            "a=b;",
        //            mfnList
        //        );
        //    IrbisRecord[] all = reader.ReadAll();
        //    Assert.AreEqual(mfnList.Length, all.Length);
        //    all = all.OrderBy(r => r.Mfn).ToArray();
        //    int mfn = mfnList[0];
        //    foreach (IrbisRecord record in all)
        //    {
        //        Assert.AreEqual(mfn, record.Mfn);
        //        mfn++;
        //    }
        //    Assert.AreEqual(mfn, mfnList[mfnList.Length-1] + 1);
        //}

        [TestMethod]
        public void TestMethod2()
        {
            int[] mfnList = Enumerable.Range(1, 6000).ToArray();
            ParallelRecordReader reader = new ParallelRecordReader
                (
                    3,
                    "host=192.168.3.2;port=6666;user=Никто;password=Нигде;db=IBIS;",
                    mfnList
                );
            IrbisRecord[] all = reader.ReadAll();
            Assert.AreEqual(mfnList.Length, all.Length);
            all = all.OrderBy(r => r.Mfn).ToArray();
            int mfn = mfnList[0];
            foreach (IrbisRecord record in all)
            {
                Assert.AreEqual(mfn, record.Mfn);
                mfn++;
            }
            Assert.AreEqual(mfn, mfnList[mfnList.Length - 1] + 1);
        }
    }
}

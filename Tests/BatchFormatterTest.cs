/* BatchFormatterTest.cs
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
    public class BatchFormatterTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (ManagedClient64 client = new ManagedClient64())
            {
                const string connectionString 
                    = "host=192.168.3.2;port=6666;user=Никто;password=Нигде;db=IBIS;";
                client.ParseConnectionString(connectionString);
                client.Connect();

                int[] mfnList = Enumerable.Range(1, 6000).ToArray();

                BatchRecordFormatter batch = new BatchRecordFormatter
                    (
                        client,
                        mfnList,
                        "@brief"
                    );
                string[] all = batch.FormatAll();
                //Assert.AreEqual(all.Length, mfnList.Length);
                Assert.IsTrue(all.Length > 0);
            }
        }
    }
}

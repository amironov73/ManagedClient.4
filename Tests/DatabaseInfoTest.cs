using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace Tests
{
    [TestClass]
    public class DatabaseInfoTest
    {
        [TestMethod]
        public void GetDatabaseInfo_1()
        {
            string connectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=ISTU;arm=A;";
            using (ManagedClient64 client = new ManagedClient64())
            {
                client.ParseConnectionString(connectionString);
                client.Connect();
                IrbisDatabaseInfo info = client.GetDatabaseInfo(client.Database);
                Assert.AreEqual(188583, info.MaxMfn);
                Assert.AreEqual(0, info.LockedRecords.Length);
                Assert.AreEqual(508, info.LogicallyDeletedRecords.Length);
                Assert.AreEqual(8842, info.PhysicallyDeletedRecords.Length);
                Assert.AreEqual(0, info.NonActualizedRecords.Length);
                Assert.IsFalse(info.DatabaseLocked);
            }
        }
    }
}

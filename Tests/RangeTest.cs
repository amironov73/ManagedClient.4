/* RangeTest.cs
 */

#region Using directives

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;
using ManagedClient.Ranges;

#endregion

namespace Tests
{
    [TestClass]
    public class RangeTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string text1 = "А100-А150,Б2,В1-В3";
            NumberRangeCollection ranges 
                = NumberRangeCollection.Parse(text1);
            string text2 = ranges.ToString();
            Assert.AreEqual(text1, text2);
        }

        [TestMethod]
        public void TestMethod2()
        {
            const string text1 = "А100-А150,Б2,В1-В3";
            NumberRangeCollection ranges
                = NumberRangeCollection.Parse(text1);
            int count = 0;
            foreach (NumberText number in ranges)
            {
                count++;
            }
            Assert.AreEqual(55, count);
        }

        [TestMethod]
        public void TestMethod3()
        {
            const string text1 = "2015/10";
            NumberText number = text1;
            number = number.Increment();
            string text2 = number.ToString();
            Assert.AreEqual("2015/11", text2);
        }
    }
}

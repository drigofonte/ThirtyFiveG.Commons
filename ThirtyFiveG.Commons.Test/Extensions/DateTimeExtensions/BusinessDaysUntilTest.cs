using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.DateTimeExtensions
{
    [TestClass]
    public class BusinessDaysUntilTest
    {
        [TestMethod]
        public void Same_day_time_span()
        {
            DateTime date = new DateTime(2017, 2, 1);
            int days = date.BusinessDaysUntil(new DateTime(2017, 2, 1));

            Assert.AreEqual(1, days);
        }

        [TestMethod]
        public void Day_time_span()
        {
            DateTime date = new DateTime(2017, 1, 2);
            int days = date.BusinessDaysUntil(new DateTime(2017, 1, 3));

            Assert.AreEqual(2, days);
        }

        [TestMethod]
        public void Week_time_span()
        {
            DateTime date = new DateTime(2017, 1, 1);
            int days = date.BusinessDaysUntil(new DateTime(2017, 1, 8));

            Assert.AreEqual(5, days);
        }

        [TestMethod]
        public void Month_time_span()
        {
            DateTime date = new DateTime(2017, 1, 1);
            int days = date.BusinessDaysUntil(new DateTime(2017, 1, 31));

            Assert.AreEqual(22, days);
        }

        [TestMethod]
        public void Year_time_span()
        {
            DateTime date = new DateTime(2017, 1, 1);
            int days = date.BusinessDaysUntil(new DateTime(2017, 12, 31));

            Assert.AreEqual(260, days);
        }
    }
}

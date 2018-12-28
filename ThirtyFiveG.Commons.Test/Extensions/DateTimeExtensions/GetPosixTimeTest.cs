using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThirtyFiveG.Commons.Extensions;

using E = ThirtyFiveG.Commons.Extensions.DateTimeExtensions;

namespace ThirtyFiveG.Commons.Test.Extensions.DateTimeExtensions
{
    [TestClass]
    public class GetPosixTimeTest
    {
        [TestMethod]
        public void GetPosixTime_epoch()
        {
            Assert.AreEqual(0, E.UnixEpoch.GetPosixTime());
        }

        [TestMethod]
        public void GetPosixTime_one_minute_later()
        {
            DateTime epochPlusOneMinute = new DateTime(1970, 1, 1, 0, 1, 0);

            Assert.AreEqual(60, epochPlusOneMinute.GetPosixTime());
        }

        [TestMethod]
        public void GetPosixTime_one_hour_later()
        {
            DateTime epochPlusOneHour = new DateTime(1970, 1, 1, 1, 0, 0);

            Assert.AreEqual(3600, epochPlusOneHour.GetPosixTime());
        }

        [TestMethod]
        public void GetPosixTime_one_day_later()
        {
            DateTime epochPlusOneDay = new DateTime(1970, 1, 2, 0, 0, 0);

            Assert.AreEqual(86400, epochPlusOneDay.GetPosixTime());
        }

        [TestMethod]
        public void GetPosixTime_one_year_later()
        {
            DateTime epochPlusOneYear = new DateTime(1971, 1, 1, 0, 0, 0);

            Assert.AreEqual(31536000, epochPlusOneYear.GetPosixTime());
        }
    }
}

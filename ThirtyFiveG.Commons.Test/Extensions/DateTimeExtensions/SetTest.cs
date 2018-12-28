using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.DateTimeExtensions
{
    [TestClass]
    public class SetTest
    {
        [TestMethod]
        public void SetHour()
        {
            int day = 1;
            int month = 1;
            int year = 2017;
            int hour = 1;
            int minute = 0;
            DateTime date = new DateTime(year, month, day, hour, minute, 0);

            int newHour = hour + 1;
            date = date.SetHour(newHour);

            Assert.AreEqual(day, date.Day);
            Assert.AreEqual(month, date.Month);
            Assert.AreEqual(year, date.Year);
            Assert.AreEqual(newHour, date.Hour);
            Assert.AreEqual(minute, date.Minute);
        }

        [TestMethod]
        public void SetMinute()
        {
            int day = 1;
            int month = 1;
            int year = 2017;
            int hour = 1;
            int minute = 0;
            DateTime date = new DateTime(year, month, day, hour, minute, 0);

            int newMinute = minute + 1;
            date = date.SetMinute(newMinute);

            Assert.AreEqual(day, date.Day);
            Assert.AreEqual(month, date.Month);
            Assert.AreEqual(year, date.Year);
            Assert.AreEqual(hour, date.Hour);
            Assert.AreEqual(newMinute, date.Minute);
        }
    }
}

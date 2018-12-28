using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.DateTimeExtensions
{
    [TestClass]
    public class GetDatesUntilTest
    {
        [TestMethod]
        public void GetDatesUntil_multiple_dates()
        {
            DateTime start = DateTime.UtcNow;
            DateTime end = start.AddDays(2);
            
            ICollection<DateTime> dates = start.GetDatesUntil(end);

            Assert.AreEqual(3, dates.Count);
            Assert.IsTrue(dates.Any(d => d.Ticks == start.Date.Ticks));
            Assert.IsTrue(dates.Any(d => d.Ticks == start.Date.Ticks + (TimeSpan.FromDays(1).Ticks * 1)));
            Assert.IsTrue(dates.Any(d => d.Ticks == start.Date.Ticks + (TimeSpan.FromDays(1).Ticks * 2)));
        }

        [TestMethod]
        public void GetDatesUntil_single_date()
        {
            DateTime start = DateTime.UtcNow;
            DateTime end = start;

            ICollection<DateTime> dates = start.GetDatesUntil(end);

            Assert.AreEqual(1, dates.Count);
            Assert.IsTrue(dates.Any(d => d.Ticks == start.Date.Ticks));
        }
    }
}

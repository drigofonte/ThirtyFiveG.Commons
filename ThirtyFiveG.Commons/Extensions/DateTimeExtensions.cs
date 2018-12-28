using System;
using System.Collections.Generic;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);

        public static double GetPosixTime(this DateTime d)
        {
            return TimeSpan.FromTicks(d.ToUniversalTime().Ticks - UnixEpoch.Ticks).TotalSeconds;
        }

        public static ICollection<DateTime> GetDatesUntil(this DateTime start, DateTime end)
        {
            ICollection<DateTime> dates = new List<DateTime>();
            for (DateTime current = start.Date; current <= end; current = current.AddDays(1))
                dates.Add(current);
            return dates;
        }

        public static DateTime SetHour(this DateTime date, int hour)
        {
            DateTime newDate = date.Date;
            newDate = newDate.AddHours(hour);
            newDate = newDate.AddMinutes(date.Minute);
            newDate = newDate.AddMilliseconds(date.Millisecond);
            return newDate;
        }

        public static DateTime SetMinute(this DateTime date, int minute)
        {
            DateTime newDate = date.Date;
            newDate = newDate.AddHours(date.Hour);
            newDate = newDate.AddMinutes(minute);
            newDate = newDate.AddMilliseconds(date.Millisecond);
            return newDate;
        }

        public static DateTime NextBusinessDate(this DateTime dateTime)
        {
            DateTime nextBusinessDate = dateTime.Date.AddDays(1);
            if (nextBusinessDate.DayOfWeek == DayOfWeek.Saturday)
                nextBusinessDate = nextBusinessDate.AddDays(2);
            else if (nextBusinessDate.DayOfWeek == DayOfWeek.Sunday)
                nextBusinessDate = nextBusinessDate.AddDays(1);
            return nextBusinessDate;
        }

        public static DateTime NextDate(this DateTime dateTime, bool ignoreWeekendDays)
        {
            if (ignoreWeekendDays)
                return dateTime.NextBusinessDate();
            else
                return dateTime.AddDays(1);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }

        public static bool MinutePrecisionEquals(this DateTime d1, DateTime d2)
        {
            return HourPrecisionEquals(d1, d2)
                    && d1.Minute == d2.Minute;
        }

        public static bool HourPrecisionEquals(this DateTime d1, DateTime d2)
        {
            return DayPrecisionEquals(d1, d2)
                && d1.Hour == d2.Hour;
        }

        public static bool DayPrecisionEquals(this DateTime d1, DateTime d2)
        {
            return d1.Day == d2.Day
                    && d1.Month == d2.Month
                    && d1.Year == d2.Year;
        }
    }
}

using System;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class LongExtensions
    {
        public const string ZeroDurationString = "00H00M";

        private const string _durationTemplate = "{0:00}H{1:00}M";

        public static string AsDurationString(this long ticks)
        {
            string label = string.Empty;
            TimeSpan span = TimeSpan.FromTicks(ticks);
            int days = span.Days;
            int hours = (span.Days * 24) + span.Hours;
            label = string.Format(_durationTemplate, hours, span.Minutes);
            return label;
        }
    }
}

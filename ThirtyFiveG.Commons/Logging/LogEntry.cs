using Newtonsoft.Json;
using System;

namespace ThirtyFiveG.Commons.Logging
{
    public abstract class LogEntry
    {
        public LogEntry(int userId)
        {
            UserID = userId;
            DateTime now = DateTime.UtcNow;
            UtcTimestamp = now.Ticks;
            UtcTimestampString = now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            LogEntryType = GetType().Name;
        }

        public string LogEntryType { get; private set; }
        public int UserID { get; private set; }
        public long UtcTimestamp { get; private set; }
        public string UtcTimestampString { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

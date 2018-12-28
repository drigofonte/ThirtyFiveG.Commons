using Newtonsoft.Json;
using System;

namespace ThirtyFiveG.Commons.Logging
{
    public class ExceptionLogEntry : LogEntry
    {
        #region Constructor
        public ExceptionLogEntry(Exception e, int userId) : base(userId)
        {
            Message = e.Message;
            StackTrace = JsonConvert.SerializeObject(e.ToString(), new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeHtml });
        }
        #endregion

        #region Public properties
        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;

namespace ThirtyFiveG.Commons.Logging
{
    public interface ILog
    {
        void Info(object o);
        void Info(object o, Exception e);
        void Debug(object o);
        void Debug(IEnumerable<object> o);
        void Debug(object o, Exception e);
        void Warn(object o);
        void Warn(object o, Exception e);
        void Fatal(object o);
        void Fatal(object o, Exception e);
    }
}

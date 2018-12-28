using System;

namespace ThirtyFiveG.Commons.Logging
{
    public class ActionCommandLogEntry : LogEntry
    {
        public ActionCommandLogEntry(string guid, Type commandParent, string commandName, TimeSpan executionTime, int userId) : this(guid, commandParent, commandName, executionTime.TotalMilliseconds, userId) { }
        public ActionCommandLogEntry(string guid, Type commandParent, string commandName, int userId) : this(guid, commandParent, commandName, 0, userId) { }
        public ActionCommandLogEntry(string guid, Type commandParent, string commandName, double executionTime, int userId) : base(userId)
        {
            CommandExecutionGuid = guid;
            CommandParent = commandParent.FullName;
            CommandName = commandName;
            ExecutionTime = executionTime;
        }

        public string CommandExecutionGuid { get; private set; }
        public string CommandParent { get; private set; }
        public string CommandName { get; private set; }
        public double ExecutionTime { get; private set; }
    }
}

using LogTest;

namespace LogTests
{
    public class TestLogFile : ILogFile
    {
        private readonly ISystemClock _systemClock;
        public List<LogLine> LogLines { get; private set; } = new List<LogLine>();

        public TestLogFile(ISystemClock systemClock)
        {
            _systemClock = systemClock;
        }

        public Task WriteLogLine(LogLine logLine)
        {
            LogLines.Add(logLine);
            return Task.CompletedTask;
        }
    }
}

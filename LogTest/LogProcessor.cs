using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogTest
{
    public class LogProcessor : ILogProcessor
    {
        private ILogFile _logFile;

        public LogProcessor(ILogFile logFile)
        {
            _logFile = logFile;
        }

        public async Task ProcessLogLines(List<LogLine> logLines)
        {
            int maxLinesPerIteration = 5;
            List<LogLine> _handled = new List<LogLine>();
            var tempLines = logLines.ToList();

            foreach (LogLine logLine in tempLines)
            {
                if (_handled.Count >= maxLinesPerIteration)
                    break;

                try
                {
                    await _logFile.WriteLogLine(logLine);
                    _handled.Add(logLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while writing log line: {ex.Message}");
                }
            }

            // Remove the handled lines from the logLines list
            logLines.RemoveAll(line => _handled.Contains(line));
        }
    }
}

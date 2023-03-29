using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogTest
{
    public class LogFile : ILogFile
    {
        // SemaphoreSlim makes sure the lock is released even if exception is thrown
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1);
        private StreamWriter _writer;
        private DateTime _currentDate;
        private ISystemClock _systemClock;

        public LogFile(ISystemClock systemClock)
        {
            _systemClock = systemClock;
            if (!Directory.Exists(@"C:\git\LogAssignment\LogResults"))
                Directory.CreateDirectory(@"C:\git\LogAssignment\LogResults");

            _currentDate = systemClock.Now;
            // creates a new StreamWriter object that appends text to an existing file or creates a new file if it doesn't exist
            _writer = File.AppendText(GetLogFilePath());
            _writer.AutoFlush = true;
            WriteLogHeader();
        }

        public async Task WriteLogLine(LogLine logLine)
        {
            await _lock.WaitAsync();
            try
            {
                if (_currentDate.Date != _systemClock.Now.Date)
                {
                    Close();
                    _currentDate = _systemClock.Now;
                    _writer = File.AppendText(GetLogFilePath());
                    _writer.AutoFlush = true;
                    WriteLogHeader();
                }

                await _writer.WriteLineAsync(FormatLogLine(logLine));
            }
            finally
            {
                _lock.Release();
            }
        }

        public void Close()
        {
            _writer?.Close();
        }

        public string GetLogFilePath()
        {
            return @"C:\git\LogAssignment\LogResults\Log" + _systemClock.Now.ToString("yyyyMMdd HHmmss fff") + ".log";
        }

        private void WriteLogHeader()
        {
            _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
        }

        private string FormatLogLine(LogLine logLine)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            stringBuilder.Append("\t");
            stringBuilder.Append(logLine.LineText());
            stringBuilder.Append("\t");
            stringBuilder.Append(Environment.NewLine);
            return stringBuilder.ToString();
        }
    }
}

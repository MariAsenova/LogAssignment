using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogTest
{
    public class Log : ILog
    {
        private Task _runTask;

        private List<LogLine> _lines = new List<LogLine>();
        private ILogProcessor _logProcessor;
        private ISystemClock _systemClock;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Log(ISystemClock systemClock, ILogProcessor logProcessor)
        {
            _systemClock = systemClock;
            _logProcessor = logProcessor;
            _runTask = PerformLogging(_cancellationTokenSource.Token);
        }

        private async Task PerformLogging(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_lines.Count > 0)
                {
                    await _logProcessor.ProcessLogLines(_lines);
                }

                try
                {
                    await Task.Delay(50, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        // Ignore exception if cancellation is requested, otherwise System.Threading.Tasks.TaskCanceledException : A task was canceled.
                        break;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public async void StopWithoutFlush()
        {
            _cancellationTokenSource.Cancel();
            await _runTask;
        }

        public async void StopWithFlush()
        {
            await _logProcessor.ProcessLogLines(_lines);
            _cancellationTokenSource.Cancel();
            await _runTask;
        }

        public void Write(string text)
        {
            _lines.Add(new LogLine { Text = text, Timestamp = _systemClock.Now });
        }
    }
}

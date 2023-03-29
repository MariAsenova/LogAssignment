using LogTest;

namespace LogTests
{
    public class LogTest
    {

        [Fact]
        public void Log_WriteAndStopWithFlush_LogsCorrectly()
        {
            // Arrange
            var clock = new TestSystemClock { Now = DateTime.Now };
            var logFile = new LogFile(clock);
            ILogProcessor logProcessor = new LogProcessor(logFile);
            var logger = new Log(clock, logProcessor);

            // Act
            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(50);
            }
            logger.StopWithFlush();

            logFile.Close();

            // Assert
            string content = ReadLogFileContent(logFile.GetLogFilePath());
            int lineCount = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.Equal(16, lineCount); // 15 log lines + 1 header line
        }

        [Fact]
        public void Log_WriteAndStopWithoutFlush_LogsIncorrectly()
        {
            // Arrange
            var clock = new TestSystemClock { Now = DateTime.Now };
            var logFile = new LogFile(clock);
            ILogProcessor logProcessor = new LogProcessor(logFile);
            var logger = new Log(clock, logProcessor);
            var numMsgToLogBeforeFlush = 25;

            // Act
            for (int i = 50; i > 0; i--)
            {
                logger.Write("Number with No flush: " + i.ToString());
                Thread.Sleep(20);

                // when logging is half way through stop the process without flush
                if (i == numMsgToLogBeforeFlush)
                    logger.StopWithoutFlush();
            }

            logFile.Close();

            // Assert
            string content = ReadLogFileContent(logFile.GetLogFilePath());
            int lineCount = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.True(lineCount - 1 < 50); // should be logging from 50 down to some number without completing it all (1 header line)
        }

        [Fact]
        public async Task Log_CreatesNewFileAfterAndBeforeMidnight_TwoLogFilesCreated()
        {
            // Arrange
            var testClock = new TestSystemClock { Now = new DateTime(2023, 03, 26, 23, 59, 59) };
            var logFile = new LogFile(testClock);

            // Act
            // Before midnight
            await logFile.WriteLogLine(new LogLine { Text = "Before midnight", Timestamp = testClock.Now });

            // Save file path before midnight
            string beforeMidnightFilePath = logFile.GetLogFilePath();

            // After midnight
            testClock.Now = new DateTime(2023, 03, 27, 0, 0, 1);

            // Log a line after midnight
            await logFile.WriteLogLine(new LogLine { Text = "After midnight", Timestamp = testClock.Now });

            // Save file path after midnight
            string afterMidnightFilePath = logFile.GetLogFilePath();

            // Assert
            Assert.NotEqual(beforeMidnightFilePath, afterMidnightFilePath);

            // Clean up
            logFile.Close();
        }

        private string ReadLogFileContent(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        /*
         TODO:
         Add a function cleaning the log dir before tests run
         */

    }
}
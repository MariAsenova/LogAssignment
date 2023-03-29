using System;

namespace LogUsers
{
    using System.Threading;

    using LogTest;

    class Program
    {
        static void Main(string[] args)
        {
            ISystemClock clock = new TestSystemClock { Now = DateTime.Now };
            ILogFile logFile = new LogFile(clock);
            ILogProcessor logProcessor = new LogProcessor(logFile);
            ILog  logger = new Log(clock, logProcessor);

            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(50);
            }

            logger.StopWithFlush();
            ISystemClock clock1 = new TestSystemClock { Now = DateTime.Now };
            ILogFile logFile1 = new LogFile(clock);
            ILogProcessor logProcessor1 = new LogProcessor(logFile1);
            ILog logger2 = new Log(clock1, logProcessor1);

            for (int i = 50; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i.ToString());
                Thread.Sleep(20);
            }

            logger2.StopWithoutFlush();

            Console.ReadLine();
        }
    }
}

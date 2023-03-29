using System.Threading.Tasks;

namespace LogTest
{
    public interface ILogFile
    {
        Task WriteLogLine(LogLine logLine);
    }
}

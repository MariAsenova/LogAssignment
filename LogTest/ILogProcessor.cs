using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogTest
{
    public interface ILogProcessor
    {
        Task ProcessLogLines(List<LogLine> logLines);
    }
}

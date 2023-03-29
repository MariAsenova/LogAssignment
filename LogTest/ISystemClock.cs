using System;

namespace LogTest
{
    public interface ISystemClock
    {
        DateTime Now { get; }
    }
}
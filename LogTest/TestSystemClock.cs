using System;

namespace LogTest
{
    public class TestSystemClock : ISystemClock
    {
        public DateTime Now { get; set; }
    }
}

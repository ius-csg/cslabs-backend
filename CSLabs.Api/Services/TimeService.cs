using System;

namespace CSLabs.Api.Services
{
    public class TimeService : ITimeService
    {
        public DateTime GetCurrentTimeUtc() => DateTime.UtcNow;
    }

    public interface ITimeService
    {
        public DateTime GetCurrentTimeUtc();
    }
    public class MockTimeService : ITimeService
    {
        private DateTime _time;
        public void SetTime(DateTime time) => _time = time;
        public DateTime GetCurrentTimeUtc() =>  _time;
    }
}
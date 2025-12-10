using System;
using System.Collections.Generic;

namespace BookItsUp.Contracts.Responses
{
    public sealed class WeeklyScheduleResponse
    {
        public List<DailySegmentResponse> Segments { get; set; } = new();
    }

    public sealed class DailySegmentResponse
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }
    }
}

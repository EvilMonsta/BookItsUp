using System;
using System.Collections.Generic;

namespace BookItsUp.Contracts.Requests
{
    public sealed class UpsertWeeklyScheduleRequest
    {
        public List<DailySegmentDto> Segments { get; set; } = new();
    }

    public sealed class DailySegmentDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using BookItsUp.Domain.Enums;

namespace BookItsUp.Contracts.Responses
{
    public sealed class ScheduleExceptionResponse
    {
        public DateOnly Date { get; set; }
        public ScheduleExceptionType Type { get; set; }
        public List<LocalTimeRangeResponse>? Ranges { get; set; }
    }

    public sealed class LocalTimeRangeResponse
    {
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using BookItsUp.Domain.Enums;

namespace BookItsUp.Contracts.Requests
{
    public sealed class UpsertScheduleExceptionRequest
    {
        public DateOnly Date { get; set; }
        public ScheduleExceptionType Type { get; set; } // Closed / OpenExtra
        public List<LocalTimeRangeDto>? Ranges { get; set; } // для OpenExtra
    }

    public sealed class LocalTimeRangeDto
    {
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }
    }
}

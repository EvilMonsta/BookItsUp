using System;

namespace BookItsUp.DataAccess.Entities
{
    // Локальное время в пределах конкретного дня (без дат, только time-of-day)
    public class LocalTimeRangeEntity
    {
        public Guid Id { get; set; }

        public Guid ScheduleExceptionId { get; set; }
        public virtual ScheduleExceptionEntity? ScheduleException { get; set; }

        public TimeSpan StartLocalTime { get; set; } // включительно
        public TimeSpan EndLocalTime { get; set; }   // эксклюзивно/включительно — как решишь на доменном уровне
    }
}
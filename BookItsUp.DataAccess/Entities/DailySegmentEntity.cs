using System;

namespace BookItsUp.DataAccess.Entities
{
    public class DailySegmentEntity
    {
        public Guid Id { get; set; }

        public Guid WeeklyScheduleId { get; set; }
        public virtual WeeklyScheduleEntity? WeeklySchedule { get; set; }

        public DayOfWeek DayOfWeek { get; set; }     // соответствует доменной DailySegment.DayOfWeek
        public TimeSpan StartLocalTime { get; set; } // 00:00–24:00 в домене
        public TimeSpan EndLocalTime { get; set; }
    }
}
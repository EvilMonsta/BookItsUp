using System;

namespace BookItsUp.DataAccess.Entities
{
    public class DailySegmentEntity
    {
        public Guid Id { get; set; }

        public Guid WeeklyScheduleId { get; set; }
        public virtual WeeklyScheduleEntity? WeeklySchedule { get; set; }

        public DayOfWeek DayOfWeek { get; set; }     
        public TimeSpan StartLocalTime { get; set; } 
        public TimeSpan EndLocalTime { get; set; }
    }
}
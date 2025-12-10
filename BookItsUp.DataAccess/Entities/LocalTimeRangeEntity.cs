using System;

namespace BookItsUp.DataAccess.Entities
{
    public class LocalTimeRangeEntity
    {
        public Guid Id { get; set; }

        public Guid ScheduleExceptionId { get; set; }
        public virtual ScheduleExceptionEntity? ScheduleException { get; set; }

        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }   
    }
}
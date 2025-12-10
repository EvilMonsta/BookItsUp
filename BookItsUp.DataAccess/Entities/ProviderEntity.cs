using System;
using System.Collections.Generic;

namespace BookItsUp.DataAccess.Entities
{
    public class ProviderEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsActive { get; set; }

        public virtual WeeklyScheduleEntity? WeeklySchedule { get; set; }
        public virtual ICollection<ScheduleExceptionEntity> ScheduleExceptions { get; set; } = new List<ScheduleExceptionEntity>();
        public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    }
}
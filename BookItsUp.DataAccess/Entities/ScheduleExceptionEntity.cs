using System;
using System.Collections.Generic;
using BookItsUp.Domain.Enums;

namespace BookItsUp.DataAccess.Entities
{
    public class ScheduleExceptionEntity
    {
        public Guid Id { get; set; }

        public Guid ProviderId { get; set; }
        public virtual ProviderEntity? Provider { get; set; }

        public DateOnly Date { get; set; }
        public ScheduleExceptionType Type { get; set; }

        public virtual ICollection<LocalTimeRangeEntity> LocalTimeRanges { get; set; } = new List<LocalTimeRangeEntity>();
    }
}
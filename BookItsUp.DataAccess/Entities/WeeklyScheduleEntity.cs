using System;
using System.Collections.Generic;

namespace BookItsUp.DataAccess.Entities
{
    // Храним как отдельную сущность (1:1 к Provider) + коллекция дневных сегментов
    public class WeeklyScheduleEntity
    {
        public Guid Id { get; set; }

        public Guid ProviderId { get; set; }
        public virtual ProviderEntity? Provider { get; set; }

        public virtual ICollection<DailySegmentEntity> Segments { get; set; } = new List<DailySegmentEntity>();
    }
}
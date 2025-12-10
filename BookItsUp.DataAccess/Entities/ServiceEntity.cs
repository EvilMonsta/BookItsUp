using System;
using System.Collections.Generic;

namespace BookItsUp.DataAccess.Entities
{
    public class ServiceEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int PriceCents { get; set; }
        public bool IsActive { get; set; }
        public int BufferBeforeMinutes { get; set; }
        public int BufferAfterMinutes { get; set; }

        public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    }
}
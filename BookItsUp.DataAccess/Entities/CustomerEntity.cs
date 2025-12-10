using System;
using System.Collections.Generic;

namespace BookItsUp.DataAccess.Entities
{
    public class CustomerEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        // Навигации
        public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    }
}
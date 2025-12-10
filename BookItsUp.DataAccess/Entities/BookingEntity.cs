using BookItsUp.Domain.Enums;

namespace BookItsUp.DataAccess.Entities
{
    public class BookingEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid ProviderId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid CustomerId { get; set; }

        public DateTimeOffset StartUtc { get; set; }
        public DateTimeOffset EndUtc { get; set; }
        public BookingStatus Status { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset UpdatedAtUtc { get; set; }
        public DateTimeOffset? HoldExpiresAtUtc { get; set; }

        // Навигации
        public virtual ProviderEntity? Provider { get; set; }
        public virtual ServiceEntity? Service { get; set; }
        public virtual CustomerEntity? Customer { get; set; }
    }
}
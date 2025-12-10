using System;
using BookItsUp.Domain.Enums;

namespace BookItsUp.Contracts.Responses
{
    public sealed class BookingResponse
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
    }
}

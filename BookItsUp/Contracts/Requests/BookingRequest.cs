using System;

namespace BookItsUp.Contracts.Requests
{
    public sealed class CreateBookingRequest
    {
        public Guid OrganizationId { get; set; }
        public Guid ProviderId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTimeOffset StartUtc { get; set; }
        public DateTimeOffset EndUtc { get; set; }
        public DateTimeOffset? HoldExpiresAtUtc { get; set; }
        public bool EnsureNoOverlap { get; set; } = true;
    }

    public sealed class UpdateBookingRequest
    {
        public DateTimeOffset StartUtc { get; set; }
        public DateTimeOffset EndUtc { get; set; }
        public DateTimeOffset? HoldExpiresAtUtc { get; set; }
        public string Status { get; set; } = "Confirmed"; 
    }
}

using System;

namespace BookItsUp.Contracts.Requests
{
    public sealed class CreateServiceRequest
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = "";
        public int DurationMinutes { get; set; }
        public int PriceCents { get; set; }
        public bool IsActive { get; set; } = true;
        public int BufferBeforeMinutes { get; set; }
        public int BufferAfterMinutes { get; set; }
    }

    public sealed class UpdateServiceRequest
    {
        public string Name { get; set; } = "";
        public int DurationMinutes { get; set; }
        public int PriceCents { get; set; }
        public bool IsActive { get; set; } = true;
        public int BufferBeforeMinutes { get; set; }
        public int BufferAfterMinutes { get; set; }
    }
}

using System;

namespace BookItsUp.Contracts.Responses
{
    public sealed class ServiceResponse
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = "";
        public int DurationMinutes { get; set; }
        public int PriceCents { get; set; }
        public bool IsActive { get; set; }
        public int BufferBeforeMinutes { get; set; }
        public int BufferAfterMinutes { get; set; }
    }
}

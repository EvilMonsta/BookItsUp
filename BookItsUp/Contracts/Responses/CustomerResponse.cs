using System;

namespace BookItsUp.Contracts.Responses
{
    public sealed class CustomerResponse
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string FullName { get; set; } = "";
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}

using System;

namespace BookItsUp.Contracts.Requests
{
    public sealed class CreateCustomerRequest
    {
        public Guid OrganizationId { get; set; }
        public string FullName { get; set; } = "";
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public sealed class UpdateCustomerRequest
    {
        public string FullName { get; set; } = "";
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}

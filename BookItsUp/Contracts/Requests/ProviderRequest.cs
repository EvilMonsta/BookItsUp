using System;

namespace BookItsUp.Contracts.Requests
{
    public sealed class CreateProviderRequest
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = "";
        public string TimeZone { get; set; } = "";
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public sealed class UpdateProviderRequest
    {
        public string Name { get; set; } = "";
        public string TimeZone { get; set; } = "";
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

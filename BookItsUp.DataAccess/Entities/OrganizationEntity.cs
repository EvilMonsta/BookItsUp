using System;

namespace BookItsUp.DataAccess.Entities
{
    public class OrganizationEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? TimeZone { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAtUtc { get; set; }
    }
}

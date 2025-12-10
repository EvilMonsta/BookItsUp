using System;

namespace BookItsUp.Domain.Models
{
    public sealed class Organization
    {
        public const int MAX_NAME_LENGTH = 200;
        public const int MAX_TIMEZONE_LENGTH = 100;

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? TimeZone { get; private set; }
        public bool IsActive { get; private set; }
        public DateTimeOffset CreatedAtUtc { get; private set; }

        private Organization() { }

        public Organization(Guid id, string name, string? timeZone, bool isActive, DateTimeOffset createdAtUtc)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty.", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (name.Length > MAX_NAME_LENGTH) throw new ArgumentOutOfRangeException(nameof(name));

            if (timeZone is { Length: > MAX_TIMEZONE_LENGTH })
                throw new ArgumentOutOfRangeException(nameof(timeZone));

            Id = id;
            Name = name.Trim();
            TimeZone = string.IsNullOrWhiteSpace(timeZone) ? null : timeZone.Trim();
            IsActive = isActive;
            CreatedAtUtc = createdAtUtc == default ? DateTimeOffset.UtcNow : createdAtUtc;
        }
    }
}

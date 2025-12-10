using BookItsUp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.Domain
{
    public class Service
    {
        public const int MAX_NAME_LENGTH = 255;

        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public int DurationMinutes { get; private set; }

        public int PriceCents { get; private set; }

        public bool IsActive { get; private set; }

        public int BufferBeforeMinutes { get; private set; }

        public int BufferAfterMinutes { get; private set; }

        public Service(Guid id, Guid organizationId, string name, int durationMinutes, int priceCents, bool isActive, int bufferBeforeMinutes, int bufferAfterMinutes)
        {

            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty.", nameof(id));
            if (organizationId == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationId));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (name.Length > 255) throw new ArgumentOutOfRangeException(nameof(name));
            if (durationMinutes <= 0)
                throw new ArgumentException("Duration should be > 0");
            if (priceCents < 0)
                throw new ArgumentException("Price should not be < 0");
            if (bufferBeforeMinutes < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferBeforeMinutes));
            if (bufferAfterMinutes < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferAfterMinutes));


            Id = id;
            OrganizationId = organizationId;
            Name = name.Trim();
            DurationMinutes = durationMinutes;
            PriceCents = priceCents;
            IsActive = isActive;
            BufferBeforeMinutes = bufferBeforeMinutes;
            BufferAfterMinutes = bufferAfterMinutes;
        }
    }
}

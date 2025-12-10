using BookItsUp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.Domain
{
    public class Booking
    {
        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public Guid ProviderId { get; private set; }

        public Guid ServiceId { get; private set; }

        public Guid CustomerId { get; private set; }

        public DateTimeOffset StartUtc { get; private set; }

        public DateTimeOffset EndUtc { get; private set; }

        public BookingStatus Status { get; private set; }

        public DateTimeOffset CreatedAtUtc { get; private set; }

        public DateTimeOffset UpdatedAtUtc { get; private set; }

        public DateTimeOffset? HoldExpiresAtUtc { get; private set; }

        public Booking(Guid id, Guid organizationId, Guid providerId, Guid serviceId, Guid customerId, DateTimeOffset startUtc, DateTimeOffset endUtc, BookingStatus status, DateTimeOffset createdAtUtc, DateTimeOffset updatedAtUtc, DateTimeOffset? holdExpiresAtUtc)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty.", nameof(id));
            if (organizationId == Guid.Empty) throw new ArgumentException("OrganizationId must not be empty.", nameof(organizationId));
            if (providerId == Guid.Empty) throw new ArgumentException("ProviderId must not be empty.", nameof(providerId));
            if (serviceId == Guid.Empty) throw new ArgumentException("ServiceId must not be empty.", nameof(serviceId));
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId must not be empty.", nameof(customerId));

            var start = startUtc.ToUniversalTime();
            var end = endUtc.ToUniversalTime();
            if (end <= start) throw new ArgumentException("EndUtc must be greater than StartUtc.");

            if (status == BookingStatus.Held)
            {
                if (!holdExpiresAtUtc.HasValue)
                    throw new ArgumentException("HoldExpiresAtUtc must be provided when status is Held.", nameof(holdExpiresAtUtc));

                var hold = holdExpiresAtUtc.Value.ToUniversalTime();
                if (hold <= DateTimeOffset.UtcNow)
                    throw new ArgumentException("HoldExpiresAtUtc must be in the future.", nameof(holdExpiresAtUtc));

                HoldExpiresAtUtc = hold;
            }
            else
            {
                if (holdExpiresAtUtc.HasValue)
                    throw new ArgumentException("HoldExpiresAtUtc must be null unless status is Held.", nameof(holdExpiresAtUtc));
                HoldExpiresAtUtc = null;
            }

            Id = id;
            OrganizationId = organizationId;
            ProviderId = providerId;
            ServiceId = serviceId;
            CustomerId = customerId;
            StartUtc = start;
            EndUtc = end;
            Status = status;

            var now = DateTimeOffset.UtcNow;
            CreatedAtUtc = now;
            UpdatedAtUtc = now;
        }

        public void Complete()
        {
            if (Status != BookingStatus.Confirmed) throw new InvalidOperationException("Only confirmed bookings can be completed.");
            Status = BookingStatus.Completed;
            UpdatedAtUtc = DateTimeOffset.UtcNow;
        }
    }
}


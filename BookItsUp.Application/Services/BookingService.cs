using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;

namespace BookItsUp.Application.Services
{
    public sealed class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IOrganizationRepository _orgs;

        public BookingService(IBookingRepository repo, IOrganizationRepository orgs)
        {
            _repo = repo;
            _orgs = orgs;
        }

        public Task<Booking?> GetAsync(Guid id, CancellationToken ct)
            => _repo.GetAsync(id, ct);

        public Task<IReadOnlyList<Booking>> ListByProviderAsync(Guid providerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
            => _repo.ListByProviderAsync(providerId, from, to, ct); 

        public Task<IReadOnlyList<Booking>> ListByCustomerAsync(Guid customerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
            => _repo.ListByCustomerAsync(customerId, from, to, ct); 

        public async Task<Booking> CreateAsync(Booking booking, CancellationToken ct, bool ensureNoOverlap = true)
        {
            var exists = await _orgs.ExistsAsync(booking.OrganizationId, ct);
            if (!exists)
                throw new InvalidOperationException("Organization does not exist.");

            if (ensureNoOverlap)
            {
                var overlap = await _repo.ExistsOverlapAsync(booking.ProviderId, booking.StartUtc, booking.EndUtc, ct);
                if (overlap)
                    throw new InvalidOperationException("Time slot overlaps with an existing booking.");
            }
            return await _repo.CreateAsync(booking, ct); 
        }

        public Task UpdateAsync(Booking booking, CancellationToken ct)
            => _repo.UpdateAsync(booking, ct); 

        public Task DeleteAsync(Guid id, CancellationToken ct)
            => _repo.DeleteAsync(id, ct);
    }
}

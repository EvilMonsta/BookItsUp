using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Domain.Abstractions
{
    public interface IBookingService
    {
        Task<Booking?> GetAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Booking>> ListByProviderAsync(Guid providerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct);
        Task<IReadOnlyList<Booking>> ListByCustomerAsync(Guid customerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct);

        Task<Booking> CreateAsync(Booking booking, CancellationToken ct, bool ensureNoOverlap = true);
        Task UpdateAsync(Booking booking, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
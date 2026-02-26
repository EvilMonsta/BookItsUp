using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookItsUp.DataAccess;
using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookItsUp.DataAccess.Repositories
{
    public sealed class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _context;
        public BookingRepository(BookingDbContext context) => _context = context;

        public async Task<Booking?> GetAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Booking>> ListByProviderAsync(Guid providerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
        {
            var list = await _context.Bookings.AsNoTracking()
                .Where(x => x.ProviderId == providerId && x.StartUtc < to && x.EndUtc > from)
                .OrderBy(x => x.StartUtc)
                .ToListAsync(ct);

            return list.Select(ToDomain).ToList();
        }

        public async Task<IReadOnlyList<Booking>> ListByCustomerAsync(Guid customerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
        {
            var list = await _context.Bookings.AsNoTracking()
                .Where(x => x.CustomerId == customerId && x.StartUtc < to && x.EndUtc > from)
                .OrderBy(x => x.StartUtc)
                .ToListAsync(ct);

            return list.Select(ToDomain).ToList();
        }

        public async Task<Booking> CreateAsync(Booking booking, CancellationToken ct)
        {
            var orgExists = await _context.Organizations
                .AsNoTracking()
                .AnyAsync(o => o.Id == booking.OrganizationId, ct);

            if (!orgExists)
                throw new InvalidOperationException("Organization does not exist.");

            var e = ToEntity(booking);
            _context.Bookings.Add(e);
            await _context.SaveChangesAsync(ct);
            return ToDomain(e);
        }

        public async Task UpdateAsync(Booking booking, CancellationToken ct)
        {
            var e = ToEntity(booking);
            _context.Bookings.Update(e);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return;
            _context.Bookings.Remove(e);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsOverlapAsync(Guid providerId, DateTimeOffset start, DateTimeOffset end, CancellationToken ct)
        {
            return await _context.Bookings
                .AnyAsync(x => x.ProviderId == providerId && x.StartUtc < end && x.EndUtc > start, ct);
        }

        private static Booking ToDomain(BookingEntity e) =>
            new Booking(
                e.Id, e.OrganizationId, e.ProviderId, e.ServiceId, e.CustomerId,
                e.StartUtc, e.EndUtc, e.Status, e.CreatedAtUtc, e.UpdatedAtUtc, e.HoldExpiresAtUtc);

        private static BookingEntity ToEntity(Booking b) => new BookingEntity
        {
            Id = b.Id,
            OrganizationId = b.OrganizationId,
            ProviderId = b.ProviderId,
            ServiceId = b.ServiceId,
            CustomerId = b.CustomerId,
            StartUtc = b.StartUtc,
            EndUtc = b.EndUtc,
            Status = b.Status,
            CreatedAtUtc = b.CreatedAtUtc,
            UpdatedAtUtc = b.UpdatedAtUtc,
            HoldExpiresAtUtc = b.HoldExpiresAtUtc
        };
    }
}

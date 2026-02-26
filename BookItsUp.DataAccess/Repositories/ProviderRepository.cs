using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookitUp.Infrastructure;
using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookItsUp.DataAccess.Repositories
{
    public sealed class ProviderRepository : IProviderRepository
    {
        private readonly BookingDbContext _context;
        public ProviderRepository(BookingDbContext context) => _context = context;

        public async Task<Provider?> GetAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Providers.AsNoTracking()
                .Include(p => p.WeeklySchedule)!.ThenInclude(ws => ws.Segments)
                .Include(p => p.ScheduleExceptions)!.ThenInclude(se => se.LocalTimeRanges)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Provider>> ListByOrganizationAsync(Guid organizationId, bool onlyActive, CancellationToken ct)
        {
            var q = _context.Providers.AsNoTracking()
                .Include(p => p.WeeklySchedule)!.ThenInclude(ws => ws.Segments)
                .Include(p => p.ScheduleExceptions)!.ThenInclude(se => se.LocalTimeRanges)
                .Where(x => x.OrganizationId == organizationId);

            if (onlyActive) q = q.Where(x => x.IsActive);

            var list = await q.OrderBy(x => x.Name).ToListAsync(ct);
            return list.Select(ToDomain).ToList();
        }

        public async Task<IReadOnlyList<Provider>> ListAsync(bool onlyActive, CancellationToken ct)
        {
            var q = _context.Providers.AsNoTracking()
                .Include(p => p.WeeklySchedule)!.ThenInclude(ws => ws.Segments)
                .Include(p => p.ScheduleExceptions)!.ThenInclude(se => se.LocalTimeRanges)
                .AsQueryable();

            if (onlyActive) q = q.Where(x => x.IsActive);

            var list = await q.OrderBy(x => x.Name).ToListAsync(ct);
            return list.Select(ToDomain).ToList();
        }

        public async Task<Provider> CreateAsync(Provider provider, CancellationToken ct)
        {
            var orgExists = await _context.Organizations
                .AsNoTracking()
                .AnyAsync(o => o.Id == provider.OrganizationId, ct);

            if (!orgExists)
                throw new InvalidOperationException("Organization does not exist.");

            var e = ToEntity(provider);
            _context.Providers.Add(e);
            await _context.SaveChangesAsync(ct);

            var saved = await _context.Providers.AsNoTracking()
                .Include(p => p.WeeklySchedule)!.ThenInclude(ws => ws.Segments)
                .Include(p => p.ScheduleExceptions)!.ThenInclude(se => se.LocalTimeRanges)
                .FirstAsync(p => p.Id == e.Id, ct);
            return ToDomain(saved);
        }

        public async Task UpdateAsync(Provider provider, CancellationToken ct)
        {
            var e = ToEntity(provider);
            _context.Providers.Update(e);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Providers.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return;
            _context.Providers.Remove(e);
            await _context.SaveChangesAsync(ct);
        }

        private static Provider ToDomain(ProviderEntity e)
        {
            var weekly = e.WeeklySchedule is null
                ? new WeeklySchedule(Array.Empty<DailySegment>())
                : new WeeklySchedule(
                    e.WeeklySchedule.Segments
                     .OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartLocalTime)
                     .Select(s => new DailySegment(s.DayOfWeek, s.StartLocalTime, s.EndLocalTime))
                     .ToList()
                  );

            var exceptions = e.ScheduleExceptions?
                .OrderBy(se => se.Date)
                .Select(se =>
                {
                    var segs = se.Type == Domain.Enums.ScheduleExceptionType.OpenExtra
                        ? se.LocalTimeRanges
                            .OrderBy(r => r.StartLocalTime)
                            .Select(r => new LocalTimeRange(r.StartLocalTime, r.EndLocalTime))
                            .ToList()
                        : null;
                    return new ScheduleException(se.Date, se.Type, segs);
                })
                .ToList() ?? new List<ScheduleException>();

            return new Provider(
                e.Id, e.OrganizationId, e.Name, e.TimeZone, e.Capacity,
                weekly, e.IsActive, exceptions
            );
        }

        private static ProviderEntity ToEntity(Provider p) => new ProviderEntity
        {
            Id = p.Id,
            OrganizationId = p.OrganizationId,
            Name = p.Name,
            TimeZone = p.TimeZone,
            Capacity = p.Capacity,
            IsActive = p.IsActive
        };
    }
}

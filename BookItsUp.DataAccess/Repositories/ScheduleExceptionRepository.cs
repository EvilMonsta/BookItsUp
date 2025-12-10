using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookitUp.Infrastructure;
using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookItsUp.DataAccess.Repositories
{
    public sealed class ScheduleExceptionRepository : IScheduleExceptionRepository
    {
        private readonly BookingDbContext _context;
        public ScheduleExceptionRepository(BookingDbContext context) => _context = context;

        public async Task<ScheduleException?> GetAsync(Guid providerId, DateOnly date, CancellationToken ct)
        {
            var e = await _context.ScheduleExceptions.AsNoTracking()
                .Include(se => se.LocalTimeRanges)
                .FirstOrDefaultAsync(se => se.ProviderId == providerId && se.Date == date, ct);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<ScheduleException>> ListByProviderAsync(Guid providerId, DateOnly from, DateOnly to, CancellationToken ct)
        {
            var list = await _context.ScheduleExceptions.AsNoTracking()
                .Include(se => se.LocalTimeRanges)
                .Where(se => se.ProviderId == providerId && se.Date >= from && se.Date <= to)
                .OrderBy(se => se.Date)
                .ToListAsync(ct);
            return list.Select(ToDomain).ToList();
        }

        public async Task UpsertAsync(Guid providerId, ScheduleException exception, CancellationToken ct)
        {
            var existing = await _context.ScheduleExceptions
                .Include(se => se.LocalTimeRanges)
                .FirstOrDefaultAsync(se => se.ProviderId == providerId && se.Date == exception.Date, ct);

            if (existing is null)
            {
                var created = ToEntity(providerId, exception);
                _context.ScheduleExceptions.Add(created);
            }
            else
            {
                existing.Type = exception.Type;

                _context.LocalTimeRanges.RemoveRange(existing.LocalTimeRanges);
                existing.LocalTimeRanges.Clear();

                if (exception.Segments is not null)
                {
                    foreach (var r in exception.Segments)
                    {
                        existing.LocalTimeRanges.Add(new LocalTimeRangeEntity
                        {
                            Id = Guid.NewGuid(),
                            ScheduleExceptionId = existing.Id,
                            StartLocalTime = r.StartLocalTime,
                            EndLocalTime = r.EndLocalTime
                        });
                    }
                }

                _context.ScheduleExceptions.Update(existing);
            }

            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid providerId, DateOnly date, CancellationToken ct)
        {
            var e = await _context.ScheduleExceptions
                .FirstOrDefaultAsync(x => x.ProviderId == providerId && x.Date == date, ct);
            if (e is null) return;
            _context.ScheduleExceptions.Remove(e);
            await _context.SaveChangesAsync(ct);
        }

        private static ScheduleException ToDomain(ScheduleExceptionEntity e)
        {
            var segments = e.Type == Domain.Enums.ScheduleExceptionType.OpenExtra
                ? e.LocalTimeRanges
                    .OrderBy(r => r.StartLocalTime)
                    .Select(r => new LocalTimeRange(r.StartLocalTime, r.EndLocalTime))
                    .ToList()
                : null;

            return new ScheduleException(e.Date, e.Type, segments);
        }

        private static ScheduleExceptionEntity ToEntity(Guid providerId, ScheduleException d) => new ScheduleExceptionEntity
        {
            Id = Guid.NewGuid(),
            ProviderId = providerId,
            Date = d.Date,
            Type = d.Type,
            LocalTimeRanges = d.Segments?.Select(r => new LocalTimeRangeEntity
            {
                Id = Guid.NewGuid(),
                StartLocalTime = r.StartLocalTime,
                EndLocalTime = r.EndLocalTime
            }).ToList() ?? new List<LocalTimeRangeEntity>()
        };
    }
}

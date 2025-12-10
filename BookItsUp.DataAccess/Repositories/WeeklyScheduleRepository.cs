using System;
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
    public sealed class WeeklyScheduleRepository : IWeeklyScheduleRepository
    {
        private readonly BookingDbContext _context;
        public WeeklyScheduleRepository(BookingDbContext context) => _context = context;

        public async Task<WeeklySchedule?> GetByProviderAsync(Guid providerId, CancellationToken ct)
        {
            var e = await _context.WeeklySchedules.AsNoTracking()
                .Include(ws => ws.Segments)
                .FirstOrDefaultAsync(ws => ws.ProviderId == providerId, ct);

            return e is null ? null : ToDomain(e);
        }

        public async Task<WeeklySchedule> CreateAsync(Guid providerId, WeeklySchedule schedule, CancellationToken ct)
        {
            var e = ToEntity(providerId, schedule);
            _context.WeeklySchedules.Add(e);
            await _context.SaveChangesAsync(ct);
            var saved = await _context.WeeklySchedules.AsNoTracking().Include(x => x.Segments).FirstAsync(x => x.Id == e.Id, ct);
            return ToDomain(saved);
        }

        public async Task UpdateAsync(Guid providerId, WeeklySchedule schedule, CancellationToken ct)
        {
            var existing = await _context.WeeklySchedules
                .Include(ws => ws.Segments)
                .FirstOrDefaultAsync(ws => ws.ProviderId == providerId, ct);

            if (existing is null)
            {
                await CreateAsync(providerId, schedule, ct);
                return;
            }

            _context.DailySegments.RemoveRange(existing.Segments);
            existing.Segments.Clear();

            foreach (var s in schedule.Segments)
            {
                existing.Segments.Add(new DailySegmentEntity
                {
                    Id = Guid.NewGuid(),
                    WeeklyScheduleId = existing.Id,
                    DayOfWeek = s.DayOfWeek,
                    StartLocalTime = s.StartLocalTime,
                    EndLocalTime = s.EndLocalTime
                });
            }

            _context.WeeklySchedules.Update(existing);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid providerId, CancellationToken ct)
        {
            var existing = await _context.WeeklySchedules.FirstOrDefaultAsync(ws => ws.ProviderId == providerId, ct);
            if (existing is null) return;
            _context.WeeklySchedules.Remove(existing);
            await _context.SaveChangesAsync(ct);
        }

        private static WeeklySchedule ToDomain(WeeklyScheduleEntity e) =>
            new WeeklySchedule(
                e.Segments
                 .OrderBy(s => s.DayOfWeek)
                 .ThenBy(s => s.StartLocalTime)
                 .Select(s => new DailySegment(s.DayOfWeek, s.StartLocalTime, s.EndLocalTime))
                 .ToList()
            );

        private static WeeklyScheduleEntity ToEntity(Guid providerId, WeeklySchedule d) => new WeeklyScheduleEntity
        {
            Id = Guid.NewGuid(),
            ProviderId = providerId,
            Segments = d.Segments.Select(s => new DailySegmentEntity
            {
                Id = Guid.NewGuid(),
                DayOfWeek = s.DayOfWeek,
                StartLocalTime = s.StartLocalTime,
                EndLocalTime = s.EndLocalTime
            }).ToList()
        };
    }
}
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
    public sealed class ServiceRepository : IServiceRepository
    {
        private readonly BookingDbContext _context;
        public ServiceRepository(BookingDbContext context) => _context = context;

        public async Task<Service?> GetAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Services.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Service>> ListByOrganizationAsync(Guid organizationId, bool onlyActive, CancellationToken ct)
        {
            var q = _context.Services.AsNoTracking().Where(x => x.OrganizationId == organizationId);
            if (onlyActive) q = q.Where(x => x.IsActive);

            var list = await q.OrderBy(x => x.Name).ToListAsync(ct);
            return list.Select(ToDomain).ToList();
        }

        public async Task<IReadOnlyList<Service>> ListAsync(bool onlyActive, CancellationToken ct)
        {
            var q = _context.Services.AsNoTracking().AsQueryable();
            if (onlyActive) q = q.Where(x => x.IsActive);

            var list = await q.OrderBy(x => x.Name).ToListAsync(ct);
            return list.Select(ToDomain).ToList();
        }

        public async Task<Service> CreateAsync(Service service, CancellationToken ct)
        {
            var orgExists = await _context.Organizations
               .AsNoTracking()
              .AnyAsync(o => o.Id == service.OrganizationId, ct);

            if (!orgExists)
                throw new InvalidOperationException("Organization does not exist.");

            var e = ToEntity(service);
            _context.Services.Add(e);
            await _context.SaveChangesAsync(ct);
            return ToDomain(e);
        }

        public async Task UpdateAsync(Service service, CancellationToken ct)
        {
            var e = ToEntity(service);
            _context.Services.Update(e);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Services.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return;
            _context.Services.Remove(e);
            await _context.SaveChangesAsync(ct);
        }

        private static Service ToDomain(ServiceEntity e) =>
            new Service(e.Id, e.OrganizationId, e.Name, e.DurationMinutes, e.PriceCents, e.IsActive, e.BufferBeforeMinutes, e.BufferAfterMinutes);

        private static ServiceEntity ToEntity(Service s) => new ServiceEntity
        {
            Id = s.Id,
            OrganizationId = s.OrganizationId,
            Name = s.Name,
            DurationMinutes = s.DurationMinutes,
            PriceCents = s.PriceCents,
            IsActive = s.IsActive,
            BufferBeforeMinutes = s.BufferBeforeMinutes,
            BufferAfterMinutes = s.BufferAfterMinutes
        };
    }
}
using BookItsUp.DataAccess.Entities;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookItsUp.Domain.Models;
using BookitUp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.DataAccess.Repositories
{
    public sealed class OrganizationRepository : IOrganizationRepository
    {
        private readonly BookingDbContext _ctx;
        public OrganizationRepository(BookingDbContext ctx) => _ctx = ctx;

        public Task<bool> ExistsAsync(Guid id, CancellationToken ct) =>
            _ctx.Organizations.AsNoTracking().AnyAsync(x => x.Id == id, ct);

        public async Task<Organization?> GetAsync(Guid id, CancellationToken ct)
        {
            var e = await _ctx.Organizations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Organization>> ListAsync(CancellationToken ct)
        {
            var list = await _ctx.Organizations.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
            return list.Select(ToDomain).ToList();
        }

        public async Task<Organization> CreateAsync(Organization org, CancellationToken ct)
        {
            var e = ToEntity(org);
            _ctx.Organizations.Add(e);
            await _ctx.SaveChangesAsync(ct);
            return ToDomain(e);
        }

        public async Task UpdateAsync(Organization org, CancellationToken ct)
        {
            var e = ToEntity(org);
            _ctx.Organizations.Update(e);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var e = await _ctx.Organizations.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return;
            _ctx.Organizations.Remove(e);
            await _ctx.SaveChangesAsync(ct);
        }

        private static Organization ToDomain(OrganizationEntity e) =>
            new Organization(e.Id, e.Name, e.TimeZone, e.IsActive, e.CreatedAtUtc);

        private static OrganizationEntity ToEntity(Organization d) => new OrganizationEntity
        {
            Id = d.Id,
            Name = d.Name,
            TimeZone = d.TimeZone,
            IsActive = d.IsActive,
            CreatedAtUtc = d.CreatedAtUtc
        };
    }
}

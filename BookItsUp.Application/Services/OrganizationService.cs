using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookItsUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Application.Services
{
    public sealed class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _repo;

        public OrganizationService(IOrganizationRepository repo) => _repo = repo;

        public Task<bool> ExistsAsync(Guid id, CancellationToken ct) => _repo.ExistsAsync(id, ct);
        public Task<Organization?> GetAsync(Guid id, CancellationToken ct) => _repo.GetAsync(id, ct);
        public Task<IReadOnlyList<Organization>> ListAsync(CancellationToken ct) => _repo.ListAsync(ct);

        public Task<Organization> CreateAsync(string name, string? timeZone, bool isActive, CancellationToken ct)
        {
            var org = new Organization(Guid.NewGuid(), name, timeZone, isActive, DateTimeOffset.UtcNow);
            return _repo.CreateAsync(org, ct);
        }

        public Task UpdateAsync(Organization org, CancellationToken ct) => _repo.UpdateAsync(org, ct);
        public Task DeleteAsync(Guid id, CancellationToken ct) => _repo.DeleteAsync(id, ct);
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;

namespace BookItsUp.Application.Services
{
    public sealed class ProviderService : IProviderService
    {
        private readonly IProviderRepository _repo;
        private readonly IOrganizationRepository _orgs;

        public ProviderService(IProviderRepository repo, IOrganizationRepository orgs)
        {
            _repo = repo;
            _orgs = orgs;
        }

        public Task<Provider?> GetAsync(Guid id, CancellationToken ct)
            => _repo.GetAsync(id, ct); 

        public Task<IReadOnlyList<Provider>> ListByOrganizationAsync(Guid organizationId, bool onlyActive, CancellationToken ct)
            => _repo.ListByOrganizationAsync(organizationId, onlyActive, ct); 

        public async Task<Provider> CreateAsync(Provider provider, CancellationToken ct)
        {
            var exists = await _orgs.ExistsAsync(provider.OrganizationId, ct);
            if (!exists)
                throw new InvalidOperationException("Organization does not exist.");

            return await _repo.CreateAsync(provider, ct);
        }

        public Task UpdateAsync(Provider provider, CancellationToken ct)
            => _repo.UpdateAsync(provider, ct); 

        public Task DeleteAsync(Guid id, CancellationToken ct)
            => _repo.DeleteAsync(id, ct); 
    }
}

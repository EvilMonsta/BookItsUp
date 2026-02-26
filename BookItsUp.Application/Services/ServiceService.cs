using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;

namespace BookItsUp.Application.Services
{
    public sealed class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repo;
        private readonly IOrganizationRepository _orgs;

        public ServiceService(IServiceRepository repo, IOrganizationRepository orgs)
        {
            _repo = repo;
            _orgs = orgs;
        }

        public Task<Service?> GetAsync(Guid id, CancellationToken ct)
            => _repo.GetAsync(id, ct); 

        public Task<IReadOnlyList<Service>> ListByOrganizationAsync(Guid organizationId, bool onlyActive, CancellationToken ct)
            => _repo.ListByOrganizationAsync(organizationId, onlyActive, ct); 

        public Task<IReadOnlyList<Service>> ListAsync(bool onlyActive, CancellationToken ct)
            => _repo.ListAsync(onlyActive, ct); 

        public async Task<Service> CreateAsync(Service service, CancellationToken ct)
        {
            var exists = await _orgs.ExistsAsync(service.OrganizationId, ct);
            if (!exists)
                throw new InvalidOperationException("Organization does not exist.");

            return await _repo.CreateAsync(service, ct);
        }

        public Task UpdateAsync(Service service, CancellationToken ct)
            => _repo.UpdateAsync(service, ct);

        public Task DeleteAsync(Guid id, CancellationToken ct)
            => _repo.DeleteAsync(id, ct); 
    }
}

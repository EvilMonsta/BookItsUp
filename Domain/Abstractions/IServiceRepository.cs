using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;

namespace BookItsUp.Domain.Abstractions
{
    public interface IServiceRepository
    {
        Task<Service?> GetAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Service>> ListByOrganizationAsync(Guid organizationId, bool onlyActive, CancellationToken ct);
        Task<IReadOnlyList<Service>> ListAsync(bool onlyActive, CancellationToken ct);

        Task<Service> CreateAsync(Service service, CancellationToken ct);
        Task UpdateAsync(Service service, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}

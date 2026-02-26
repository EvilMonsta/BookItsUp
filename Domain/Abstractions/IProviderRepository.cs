using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;

namespace BookItsUp.Domain.Abstractions
{
    public interface IProviderRepository
    {
        Task<Provider?> GetAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Provider>> ListByOrganizationAsync(Guid organizationId, bool onlyActive, CancellationToken ct);
        Task<IReadOnlyList<Provider>> ListAsync(bool onlyActive, CancellationToken ct);

        Task<Provider> CreateAsync(Provider provider, CancellationToken ct);
        Task UpdateAsync(Provider provider, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
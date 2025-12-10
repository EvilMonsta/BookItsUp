using BookItsUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Domain.Abstractions
{
    public interface IOrganizationRepository
    {
        Task<bool> ExistsAsync(Guid id, CancellationToken ct);
        Task<Organization?> GetAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Organization>> ListAsync(CancellationToken ct);

        Task<Organization> CreateAsync(Organization org, CancellationToken ct);
        Task UpdateAsync(Organization org, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}

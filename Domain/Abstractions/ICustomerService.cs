using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Domain.Abstractions
{
    public interface ICustomerService
    {
        Task<Customer?> GetAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Customer>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct);
        Task<IReadOnlyList<Customer>> ListAsync(CancellationToken ct);
        Task<Customer?> SearchByAnyAsync(string? name, string? email, string? phone, CancellationToken ct);

        Task<Customer> CreateAsync(Customer customer, CancellationToken ct);
        Task UpdateAsync(Customer customer, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}

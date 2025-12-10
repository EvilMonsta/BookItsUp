using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Application.Services
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IOrganizationRepository _orgs;

        public CustomerService(ICustomerRepository repo, IOrganizationRepository orgs)
        {
            _repo = repo;
            _orgs = orgs;
        }

        public Task<Customer?> GetAsync(Guid id, CancellationToken ct)
            => _repo.GetAsync(id, ct);

        public Task<Customer?> SearchByAnyAsync(string? name, string? email, string? phone, CancellationToken ct)
            => _repo.SearchByAnyAsync(name, email, phone, ct);

        public Task<IReadOnlyList<Customer>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct)
            => _repo.ListByOrganizationAsync(organizationId, ct);

        public async Task<Customer> CreateAsync(Customer customer, CancellationToken ct)
        {
            var exists = await _orgs.ExistsAsync(customer.OrganizationId, ct);
            if (!exists)
                throw new InvalidOperationException("Organization does not exist.");

            return await _repo.CreateAsync(customer, ct);
        }

        public Task UpdateAsync(Customer customer, CancellationToken ct)
            => _repo.UpdateAsync(customer, ct); 

        public Task DeleteAsync(Guid id, CancellationToken ct)
            => _repo.DeleteAsync(id, ct);
    }
}

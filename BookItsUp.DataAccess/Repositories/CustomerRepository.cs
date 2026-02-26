using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookitUp.Infrastructure;
using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookItsUp.DataAccess.Repositories
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly BookingDbContext _context;
        public CustomerRepository(BookingDbContext context) => _context = context;

        public async Task<Customer?> GetAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Customer?> SearchByAnyAsync(string? name, string? email, string? phoneNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
            {
                return null;
            }

            var query = _context.Customers.AsNoTracking();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(name) && x.FullName == name) ||
                (!string.IsNullOrWhiteSpace(email) && x.Email == email) ||
                (!string.IsNullOrWhiteSpace(phoneNumber) && x.Phone == phoneNumber)
            );

            var e = await query.FirstOrDefaultAsync(ct);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Customer>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct)
        {
            var list = await _context.Customers.AsNoTracking()
                .Where(x => x.OrganizationId == organizationId)
                .OrderBy(x => x.FullName)
                .ToListAsync(ct);

            return list.Select(ToDomain).ToList();
        }

        public async Task<IReadOnlyList<Customer>> ListAsync(CancellationToken ct)
        {
            var list = await _context.Customers.AsNoTracking()
                .OrderBy(x => x.FullName)
                .ToListAsync(ct);

            return list.Select(ToDomain).ToList();
        }

        public async Task<Customer> CreateAsync(Customer customer, CancellationToken ct)
        {
            var orgExists = await _context.Organizations
                .AsNoTracking()
                .AnyAsync(o => o.Id == customer.OrganizationId, ct);

            if (!orgExists)
                throw new InvalidOperationException("Organization does not exist.");

            var e = ToEntity(customer);
            _context.Customers.Add(e);
            await _context.SaveChangesAsync(ct);
            return ToDomain(e);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken ct)
        {
            var e = ToEntity(customer);
            _context.Customers.Update(e);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var e = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return;
            _context.Customers.Remove(e);
            await _context.SaveChangesAsync(ct);
        }

        private static Customer ToDomain(CustomerEntity e) =>
            new Customer(e.Id, e.OrganizationId, e.FullName, e.Email, e.Phone);

        private static CustomerEntity ToEntity(Customer c) => new CustomerEntity
        {
            Id = c.Id,
            OrganizationId = c.OrganizationId,
            FullName = c.FullName,
            Email = c.Email,
            Phone = c.Phone
        };
    }
}
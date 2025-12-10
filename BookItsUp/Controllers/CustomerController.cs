using BookItsUp.Contracts.Mappers;
using BookItsUp.Contracts.Requests;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomersController(ICustomerService service) => _service = service;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var c = await _service.GetAsync(id, ct);
            return c is null ? NotFound() : Ok(c.ToResponse());
        }

        [HttpGet("org/{organizationId:guid}")]
        public async Task<IActionResult> List(Guid organizationId, CancellationToken ct)
        {
            var list = await _service.ListByOrganizationAsync(organizationId, ct);
            return Ok(list.Select(x => x.ToResponse()));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomer(
            [FromQuery] string? name,
            [FromQuery] string? email,
            [FromQuery] string? phone,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
                return BadRequest("At least one search parameter (name, email, or phone) must be provided.");

            var customer = await _service.SearchByAnyAsync(name, email, phone, ct);
            return customer is null ? NotFound() : Ok(customer.ToResponse());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerRequest req, CancellationToken ct)
        {
            try
            {
                var c = new Customer(Guid.NewGuid(), req.OrganizationId, req.FullName, req.Email, req.Phone);
                var saved = await _service.CreateAsync(c, ct);
                return CreatedAtAction(nameof(Get), new { id = saved.Id }, saved.ToResponse());
            }
            catch (InvalidOperationException ex) when (ex.Message == "Organization does not exist.")
            {
                return BadRequest(new { field = "organizationId", message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateCustomerRequest req, CancellationToken ct)
        {
            var existing = await _service.GetAsync(id, ct);
            if (existing is null) return NotFound();

            var updated = new Customer(existing.Id, existing.OrganizationId, req.FullName, req.Email, req.Phone);
            await _service.UpdateAsync(updated, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}

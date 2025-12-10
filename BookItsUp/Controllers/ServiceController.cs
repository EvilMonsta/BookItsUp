using BookItsUp.Contracts.Mappers;
using BookItsUp.Contracts.Requests;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _service;
        public ServicesController(IServiceService service) => _service = service;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var s = await _service.GetAsync(id, ct);
            return s is null ? NotFound() : Ok(s.ToResponse());
        }

        [HttpGet("org/{organizationId:guid}")]
        public async Task<IActionResult> List(Guid organizationId, [FromQuery] bool onlyActive, CancellationToken ct)
        {
            var list = await _service.ListByOrganizationAsync(organizationId, onlyActive, ct);
            return Ok(list.Select(x => x.ToResponse()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceRequest req, CancellationToken ct)
        {
            var s = new Service(Guid.NewGuid(), req.OrganizationId, req.Name, req.DurationMinutes, req.PriceCents, req.IsActive, req.BufferBeforeMinutes, req.BufferAfterMinutes);
            var saved = await _service.CreateAsync(s, ct);
            return CreatedAtAction(nameof(Get), new { id = saved.Id }, saved.ToResponse());
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateServiceRequest req, CancellationToken ct)
        {
            var existing = await _service.GetAsync(id, ct);
            if (existing is null) return NotFound();

            var s = new Service(existing.Id, existing.OrganizationId, req.Name, req.DurationMinutes, req.PriceCents, req.IsActive, req.BufferBeforeMinutes, req.BufferAfterMinutes);
            await _service.UpdateAsync(s, ct);
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

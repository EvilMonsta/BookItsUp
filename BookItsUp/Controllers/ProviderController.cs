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
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _service;
        public ProvidersController(IProviderService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ListAll([FromQuery] bool onlyActive, CancellationToken ct)
        {
            var list = await _service.ListAsync(onlyActive, ct);
            return Ok(list.Select(x => x.ToResponse()));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var p = await _service.GetAsync(id, ct);
            return p is null ? NotFound() : Ok(p.ToResponse());
        }

        [HttpGet("org/{organizationId:guid}")]
        public async Task<IActionResult> List(Guid organizationId, [FromQuery] bool onlyActive, CancellationToken ct)
        {
            var list = await _service.ListByOrganizationAsync(organizationId, onlyActive, ct);
            return Ok(list.Select(x => x.ToResponse()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProviderRequest req, CancellationToken ct)
        {
            try
            {
                var emptySchedule = new WeeklySchedule(Array.Empty<DailySegment>());
                var p = new Provider(Guid.NewGuid(), req.OrganizationId, req.Name, req.TimeZone, req.Capacity, emptySchedule, req.IsActive, null);
                var saved = await _service.CreateAsync(p, ct);
                return CreatedAtAction(nameof(Get), new { id = saved.Id }, saved.ToResponse());
            }
            catch (InvalidOperationException ex) when (ex.Message == "Organization does not exist.")
            {
                return BadRequest(new { field = "organizationId", message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateProviderRequest req, CancellationToken ct)
        {
            var existing = await _service.GetAsync(id, ct);
            if (existing is null) return NotFound();

            var p = new Provider(existing.Id, existing.OrganizationId, req.Name, req.TimeZone, req.Capacity, existing.WeeklySchedule, req.IsActive, existing.ScheduleExceptions);
            await _service.UpdateAsync(p, ct);
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

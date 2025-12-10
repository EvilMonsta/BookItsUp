using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookItsUp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _svc;
        public OrganizationsController(IOrganizationService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken ct)
            => Ok((await _svc.ListAsync(ct)).Select(x => new { x.Id, x.Name, x.TimeZone, x.IsActive }));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
            => (await _svc.GetAsync(id, ct)) is { } o ? Ok(o) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrgRequest r, CancellationToken ct)
        {
            var o = await _svc.CreateAsync(r.Name, r.TimeZone, r.IsActive, ct);
            return CreatedAtAction(nameof(Get), new { id = o.Id }, o);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrgRequest r, CancellationToken ct)
        {
            var existing = await _svc.GetAsync(id, ct);
            if (existing is null) return NotFound();
            var updated = new Organization(existing.Id, r.Name, r.TimeZone, r.IsActive, existing.CreatedAtUtc);
            await _svc.UpdateAsync(updated, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        { await _svc.DeleteAsync(id, ct); return NoContent(); }

        public sealed class OrgRequest { public string Name { get; set; } = ""; public string? TimeZone { get; set; } public bool IsActive { get; set; } = true; }
    }
}

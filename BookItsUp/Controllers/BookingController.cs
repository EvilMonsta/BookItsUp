using BookItsUp.Contracts.Mappers;
using BookItsUp.Contracts.Requests;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;
using BookItsUp.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;
        public BookingsController(IBookingService service) => _service = service;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var b = await _service.GetAsync(id, ct);
            return b is null ? NotFound() : Ok(b.ToResponse());
        }

        [HttpGet("provider/{providerId:guid}")]
        public async Task<IActionResult> ListByProvider(Guid providerId, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to, CancellationToken ct)
        {
            var list = await _service.ListByProviderAsync(providerId, from, to, ct);
            return Ok(list.Select(x => x.ToResponse()));
        }

        [HttpGet("customer/{customerId:guid}")]
        public async Task<IActionResult> ListByCustomer(Guid customerId, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to, CancellationToken ct)
        {
            var list = await _service.ListByCustomerAsync(customerId, from, to, ct);
            return Ok(list.Select(x => x.ToResponse()));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest req, CancellationToken ct)
        {
            try
            {
                var status = req.HoldExpiresAtUtc.HasValue ? BookingStatus.Held : BookingStatus.Confirmed;
                var now = DateTimeOffset.UtcNow;

                var booking = new Booking(
                    id: Guid.NewGuid(),
                    organizationId: req.OrganizationId,
                    providerId: req.ProviderId,
                    serviceId: req.ServiceId,
                    customerId: req.CustomerId,
                    startUtc: req.StartUtc,
                    endUtc: req.EndUtc,
                    status: status,
                    createdAtUtc: now,
                    updatedAtUtc: now,
                    holdExpiresAtUtc: req.HoldExpiresAtUtc
                );

                var saved = await _service.CreateAsync(booking, ct, req.EnsureNoOverlap);
                return CreatedAtAction(nameof(Get), new { id = saved.Id }, saved.ToResponse());
            }
            catch (InvalidOperationException ex) when (ex.Message == "Organization does not exist.")
            {
                return BadRequest(new { field = "organizationId", message = ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message == "Time slot overlaps with an existing booking.")
            {
                return BadRequest(new { field = "timeRange", message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                var field = string.IsNullOrWhiteSpace(ex.ParamName) ? "request" : char.ToLowerInvariant(ex.ParamName[0]) + ex.ParamName.Substring(1);
                return BadRequest(new { field, message = ex.Message });
            }
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookingRequest req, CancellationToken ct)
        {
            var existing = await _service.GetAsync(id, ct);
            if (existing is null) return NotFound();

            try
            {
                var status = Enum.TryParse<BookingStatus>(req.Status, true, out var parsed)
                    ? parsed
                    : existing.Status;

                var now = DateTimeOffset.UtcNow;

                var updated = new Booking(
                    id: existing.Id,
                    organizationId: existing.OrganizationId,
                    providerId: existing.ProviderId,
                    serviceId: existing.ServiceId,
                    customerId: existing.CustomerId,
                    startUtc: req.StartUtc,
                    endUtc: req.EndUtc,
                    status: status,
                    createdAtUtc: existing.CreatedAtUtc,
                    updatedAtUtc: now,
                    holdExpiresAtUtc: req.HoldExpiresAtUtc
                );

                await _service.UpdateAsync(updated, ct);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                var field = string.IsNullOrWhiteSpace(ex.ParamName) ? "request" : char.ToLowerInvariant(ex.ParamName[0]) + ex.ParamName.Substring(1);
                return BadRequest(new { field, message = ex.Message });
            }
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}

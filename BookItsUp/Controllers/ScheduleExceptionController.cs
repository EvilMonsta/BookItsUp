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
    [Route("api/providers/{providerId:guid}/[controller]")]
    public class ScheduleExceptionsController : ControllerBase
    {
        private readonly IScheduleExceptionService _service;
        public ScheduleExceptionsController(IScheduleExceptionService service) => _service = service;

        [HttpGet("{date}")]
        public async Task<IActionResult> Get(Guid providerId, DateOnly date, CancellationToken ct)
        {
            var ex = await _service.GetAsync(providerId, date, ct);
            return ex is null ? NotFound() : Ok(ex.ToResponse());
        }

        [HttpGet]
        public async Task<IActionResult> List(Guid providerId, [FromQuery] DateOnly from, [FromQuery] DateOnly to, CancellationToken ct)
        {
            var list = await _service.ListByProviderAsync(providerId, from, to, ct);
            return Ok(list.Select(x => x.ToResponse()));
        }

        [HttpPut]
        public async Task<IActionResult> Upsert(Guid providerId, UpsertScheduleExceptionRequest req, CancellationToken ct)
        {
            var ranges = req.Ranges?.Select(r => new LocalTimeRange(r.StartLocalTime, r.EndLocalTime)).ToList();
            var exception = new ScheduleException(req.Date, req.Type, ranges);
            await _service.UpsertAsync(providerId, exception, ct);
            return NoContent();
        }

        [HttpDelete("{date}")]
        public async Task<IActionResult> Delete(Guid providerId, DateOnly date, CancellationToken ct)
        {
            await _service.DeleteAsync(providerId, date, ct);
            return NoContent();
        }
    }
}

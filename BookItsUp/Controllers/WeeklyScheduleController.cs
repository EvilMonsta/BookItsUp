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
    public class WeeklySchedulesController : ControllerBase
    {
        private readonly IWeeklyScheduleService _service;
        private readonly IProviderService _providerService;

        public WeeklySchedulesController(IWeeklyScheduleService service, IProviderService providerService)
        {
            _service = service;
            _providerService = providerService;
        }

        [HttpGet("~/api/weekly-schedules/providers")]
        public async Task<IActionResult> ListByProviders(CancellationToken ct)
        {
            var providers = await _providerService.ListAsync(false, ct);

            var result = providers
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    providerId = x.Id,
                    providerName = x.Name,
                    organizationId = x.OrganizationId,
                    weeklySchedule = x.WeeklySchedule.ToResponse()
                });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid providerId, CancellationToken ct)
        {
            var ws = await _service.GetByProviderAsync(providerId, ct);
            return ws is null ? NotFound() : Ok(ws.ToResponse());
        }

        [HttpPut]
        public async Task<IActionResult> Upsert(Guid providerId, UpsertWeeklyScheduleRequest req, CancellationToken ct)
        {
            var schedule = new WeeklySchedule(req.Segments.Select(s => new DailySegment(s.DayOfWeek, s.StartLocalTime, s.EndLocalTime)));
            var existing = await _service.GetByProviderAsync(providerId, ct);
            if (existing is null)
                await _service.CreateAsync(providerId, schedule, ct);
            else
                await _service.UpdateAsync(providerId, schedule, ct);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid providerId, CancellationToken ct)
        {
            await _service.DeleteAsync(providerId, ct);
            return NoContent();
        }
    }
}

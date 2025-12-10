using BookItsUp.Contracts.Responses;
using System.Linq;

namespace BookItsUp.Contracts.Mappers
{
    public static class DomainToResponseMapper
    {
        public static BookingResponse ToResponse(this BookItsUp.Domain.Booking b) => new()
        {
            Id = b.Id,
            OrganizationId = b.OrganizationId,
            ProviderId = b.ProviderId,
            ServiceId = b.ServiceId,
            CustomerId = b.CustomerId,
            StartUtc = b.StartUtc,
            EndUtc = b.EndUtc,
            Status = b.Status,
            CreatedAtUtc = b.CreatedAtUtc,
            UpdatedAtUtc = b.UpdatedAtUtc,
            HoldExpiresAtUtc = b.HoldExpiresAtUtc
        };

        public static CustomerResponse ToResponse(this BookItsUp.Domain.Customer c) => new()
        {
            Id = c.Id,
            OrganizationId = c.OrganizationId,
            FullName = c.FullName,
            Email = c.Email,
            Phone = c.Phone
        };

        public static ServiceResponse ToResponse(this BookItsUp.Domain.Service s) => new()
        {
            Id = s.Id,
            OrganizationId = s.OrganizationId,
            Name = s.Name,
            DurationMinutes = s.DurationMinutes,
            PriceCents = s.PriceCents,
            IsActive = s.IsActive,
            BufferBeforeMinutes = s.BufferBeforeMinutes,
            BufferAfterMinutes = s.BufferAfterMinutes
        };

        public static WeeklyScheduleResponse ToResponse(this BookItsUp.Domain.WeeklySchedule ws) => new()
        {
            Segments = ws.Segments
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartLocalTime)
                .Select(x => new DailySegmentResponse
                {
                    DayOfWeek = x.DayOfWeek,
                    StartLocalTime = x.StartLocalTime,
                    EndLocalTime = x.EndLocalTime
                }).ToList()
        };

        public static ScheduleExceptionResponse ToResponse(this BookItsUp.Domain.ScheduleException ex) => new()
        {
            Date = ex.Date,
            Type = ex.Type,
            Ranges = ex.Segments?.Select(r => new LocalTimeRangeResponse
            {
                StartLocalTime = r.StartLocalTime,
                EndLocalTime = r.EndLocalTime
            }).ToList()
        };

        public static ProviderResponse ToResponse(this BookItsUp.Domain.Provider p) => new()
        {
            Id = p.Id,
            OrganizationId = p.OrganizationId,
            Name = p.Name,
            TimeZone = p.TimeZone,
            Capacity = p.Capacity,
            IsActive = p.IsActive,
            WeeklySchedule = p.WeeklySchedule.ToResponse(),
            ScheduleExceptions = p.ScheduleExceptions.Select(ToResponse).ToList()
        };
    }
}

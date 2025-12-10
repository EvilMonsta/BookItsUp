using BookItsUp.Contracts.Responses;
using System;
using System.Collections.Generic;

namespace BookItsUp.Contracts.Responses
{
    public sealed class ProviderResponse
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = "";
        public string TimeZone { get; set; } = "";
        public int Capacity { get; set; }
        public bool IsActive { get; set; }

        public WeeklyScheduleResponse WeeklySchedule { get; set; } = new WeeklyScheduleResponse();
        public List<ScheduleExceptionResponse> ScheduleExceptions { get; set; } = new();
    }
}

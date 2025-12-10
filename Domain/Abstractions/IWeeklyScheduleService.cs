using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Domain.Abstractions
{
    public interface IWeeklyScheduleService
    {
        Task<WeeklySchedule?> GetByProviderAsync(Guid providerId, CancellationToken ct);
        Task<WeeklySchedule> CreateAsync(Guid providerId, WeeklySchedule schedule, CancellationToken ct);
        Task UpdateAsync(Guid providerId, WeeklySchedule schedule, CancellationToken ct);
        Task DeleteAsync(Guid providerId, CancellationToken ct);
    }
}

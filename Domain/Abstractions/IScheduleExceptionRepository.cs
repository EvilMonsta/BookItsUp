using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookItsUp.Domain.Abstractions
{
    public interface IScheduleExceptionRepository
    {
        Task<ScheduleException?> GetAsync(Guid providerId, DateOnly date, CancellationToken ct);
        Task<IReadOnlyList<ScheduleException>> ListByProviderAsync(Guid providerId, DateOnly from, DateOnly to, CancellationToken ct);

        Task UpsertAsync(Guid providerId, ScheduleException exception, CancellationToken ct); // создать/обновить по (providerId, date)
        Task DeleteAsync(Guid providerId, DateOnly date, CancellationToken ct);
    }
}
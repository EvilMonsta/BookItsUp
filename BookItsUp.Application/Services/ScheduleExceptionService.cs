using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;

namespace BookItsUp.Application.Services
{
    public sealed class ScheduleExceptionService : IScheduleExceptionService
    {
        private readonly IScheduleExceptionRepository _repo;

        public ScheduleExceptionService(IScheduleExceptionRepository repo) => _repo = repo;

        public Task<ScheduleException?> GetAsync(Guid providerId, DateOnly date, CancellationToken ct)
            => _repo.GetAsync(providerId, date, ct); 

        public Task<IReadOnlyList<ScheduleException>> ListByProviderAsync(Guid providerId, DateOnly from, DateOnly to, CancellationToken ct)
            => _repo.ListByProviderAsync(providerId, from, to, ct); 

        public Task UpsertAsync(Guid providerId, ScheduleException exception, CancellationToken ct)
            => _repo.UpsertAsync(providerId, exception, ct); 

        public Task DeleteAsync(Guid providerId, DateOnly date, CancellationToken ct)
            => _repo.DeleteAsync(providerId, date, ct); 
    }
}

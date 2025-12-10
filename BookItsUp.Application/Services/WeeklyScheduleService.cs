using System;
using System.Threading;
using System.Threading.Tasks;
using BookItsUp.Domain;
using BookItsUp.Domain.Abstractions;

namespace BookItsUp.Application.Services
{
    public sealed class WeeklyScheduleService : IWeeklyScheduleService
    {
        private readonly IWeeklyScheduleRepository _repo;

        public WeeklyScheduleService(IWeeklyScheduleRepository repo) => _repo = repo;

        public Task<WeeklySchedule?> GetByProviderAsync(Guid providerId, CancellationToken ct)
            => _repo.GetByProviderAsync(providerId, ct); 

        public Task<WeeklySchedule> CreateAsync(Guid providerId, WeeklySchedule schedule, CancellationToken ct)
            => _repo.CreateAsync(providerId, schedule, ct); 

        public Task UpdateAsync(Guid providerId, WeeklySchedule schedule, CancellationToken ct)
            => _repo.UpdateAsync(providerId, schedule, ct); 

        public Task DeleteAsync(Guid providerId, CancellationToken ct)
            => _repo.DeleteAsync(providerId, ct); 
    }
}

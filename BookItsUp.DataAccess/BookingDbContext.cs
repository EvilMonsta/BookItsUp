
using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.DataAccess
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }
       
        public DbSet<ServiceEntity> Services => Set<ServiceEntity>();
        public DbSet<ProviderEntity> Providers => Set<ProviderEntity>();
        public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
        public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
        public DbSet<WeeklyScheduleEntity> WeeklySchedules => Set<WeeklyScheduleEntity>();
        public DbSet<DailySegmentEntity> DailySegments => Set<DailySegmentEntity>();
        public DbSet<ScheduleExceptionEntity> ScheduleExceptions => Set<ScheduleExceptionEntity>();
        public DbSet<LocalTimeRangeEntity> LocalTimeRanges => Set<LocalTimeRangeEntity>();
        public DbSet<OrganizationEntity> Organizations => Set<OrganizationEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);

            modelBuilder.Ignore<BookItsUp.Domain.DailySegment>();
            modelBuilder.Ignore<BookItsUp.Domain.LocalTimeRange>();
            modelBuilder.Ignore<BookItsUp.Domain.WeeklySchedule>();
            modelBuilder.Ignore<BookItsUp.Domain.ScheduleException>(); 
        }

    }
}

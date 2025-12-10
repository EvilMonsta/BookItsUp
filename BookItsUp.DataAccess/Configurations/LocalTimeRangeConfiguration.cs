using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookItsUp.DataAccess.Configurations
{
    public class LocalTimeRangeConfiguration : IEntityTypeConfiguration<LocalTimeRangeEntity>
    {
        public void Configure(EntityTypeBuilder<LocalTimeRangeEntity> builder)
        {
            builder.ToTable("local_time_ranges");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.ScheduleExceptionId).IsRequired();

            builder.Property(x => x.StartLocalTime).IsRequired();
            builder.Property(x => x.EndLocalTime).IsRequired();

            builder.HasIndex(x => new
            {
                x.ScheduleExceptionId,
                x.StartLocalTime,
                x.EndLocalTime
            }).IsUnique(); 
        }
    }
}

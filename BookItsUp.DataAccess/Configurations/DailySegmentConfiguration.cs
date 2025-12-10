using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookItsUp.DataAccess.Configurations
{
    public class DailySegmentConfiguration : IEntityTypeConfiguration<DailySegmentEntity>
    {
        public void Configure(EntityTypeBuilder<DailySegmentEntity> builder)
        {
            builder.ToTable("daily_segments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.WeeklyScheduleId).IsRequired();

            builder.Property(x => x.DayOfWeek)
                   .HasConversion<int>() 
                   .IsRequired();

            builder.Property(x => x.StartLocalTime).IsRequired();
            builder.Property(x => x.EndLocalTime).IsRequired();

            builder.HasIndex(x => new
            {
                x.WeeklyScheduleId,
                x.DayOfWeek,
                x.StartLocalTime,
                x.EndLocalTime
            }).IsUnique();
        }
    }
}

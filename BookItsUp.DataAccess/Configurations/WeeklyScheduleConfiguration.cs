using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookItsUp.DataAccess.Configurations
{
    public class WeeklyScheduleConfiguration : IEntityTypeConfiguration<WeeklyScheduleEntity>
    {
        public void Configure(EntityTypeBuilder<WeeklyScheduleEntity> builder)
        {
            builder.ToTable("weekly_schedules");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.ProviderId).IsRequired();

            builder.HasIndex(x => x.ProviderId).IsUnique();

            builder.HasMany(x => x.Segments)
                   .WithOne(s => s.WeeklySchedule!)
                   .HasForeignKey(s => s.WeeklyScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

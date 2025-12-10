using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookItsUp.DataAccess.Configurations
{
    public class ScheduleExceptionConfiguration : IEntityTypeConfiguration<ScheduleExceptionEntity>
    {
        public void Configure(EntityTypeBuilder<ScheduleExceptionEntity> builder)
        {
            builder.ToTable("schedule_exceptions");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.ProviderId).IsRequired();

            builder.Property(x => x.Date)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(x => x.Type)
                   .HasConversion<string>() 
                   .IsRequired();

            builder.HasIndex(x => new { x.ProviderId, x.Date }).IsUnique();

            builder.HasMany(x => x.LocalTimeRanges)
                   .WithOne(r => r.ScheduleException!)
                   .HasForeignKey(r => r.ScheduleExceptionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

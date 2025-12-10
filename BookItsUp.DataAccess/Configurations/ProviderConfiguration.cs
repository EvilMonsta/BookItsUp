using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookItsUp.DataAccess.Configurations
{
    public class ProviderConfiguration : IEntityTypeConfiguration<ProviderEntity>
    {
        public void Configure(EntityTypeBuilder<ProviderEntity> builder)
        {
            builder.ToTable("providers");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.TimeZone)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Capacity).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasIndex(x => new { x.OrganizationId, x.Name }).IsUnique();

            builder.HasOne(x => x.WeeklySchedule)
                   .WithOne(ws => ws.Provider)
                   .HasForeignKey<WeeklyScheduleEntity>(ws => ws.ProviderId)
                   .OnDelete(DeleteBehavior.Cascade);
       
            builder.HasMany(x => x.ScheduleExceptions)
                   .WithOne(se => se.Provider!)
                   .HasForeignKey(se => se.ProviderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<OrganizationEntity>()
                   .WithMany()
                   .HasForeignKey(x => x.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property<uint>("xmin")
                   .HasColumnName("xmin")
                   .IsRowVersion();
        }
    }
}

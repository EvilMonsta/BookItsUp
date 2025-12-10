using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookItsUp.DataAccess.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder.ToTable("services");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.DurationMinutes).IsRequired();
            builder.Property(x => x.PriceCents).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.BufferBeforeMinutes).IsRequired();
            builder.Property(x => x.BufferAfterMinutes).IsRequired();

            builder.HasIndex(x => new { x.OrganizationId, x.Name }).IsUnique();

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

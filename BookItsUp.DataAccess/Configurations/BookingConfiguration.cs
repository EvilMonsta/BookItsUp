using BookItsUp.DataAccess.Entities;
using BookItsUp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

namespace BookItsUp.DataAccess.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<BookingEntity>
    {
        public void Configure(EntityTypeBuilder<BookingEntity> builder)
        {
            builder.ToTable("bookings");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.OrganizationId).IsRequired();
            builder.Property(b => b.ProviderId).IsRequired();
            builder.Property(b => b.ServiceId).IsRequired();
            builder.Property(b => b.CustomerId).IsRequired();

            builder.Property(b => b.StartUtc).IsRequired();
            builder.Property(b => b.EndUtc).IsRequired();

            builder.Property(b => b.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(b => b.HoldExpiresAtUtc);

            builder.Property(b => b.CreatedAtUtc).IsRequired();
            builder.Property(b => b.UpdatedAtUtc).IsRequired();

            builder.HasOne(b => b.Provider)
                  .WithMany(p => p.Bookings)
                  .HasForeignKey(b => b.ProviderId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Service)
                   .WithMany(s => s.Bookings)
                   .HasForeignKey(b => b.ServiceId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Customer)
                   .WithMany(c => c.Bookings)
                   .HasForeignKey(b => b.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(b => new { b.ProviderId, b.StartUtc });
            builder.HasIndex(b => new { b.CustomerId, b.StartUtc });

            builder.HasOne<OrganizationEntity>()
                   .WithMany()
                   .HasForeignKey(b => b.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property<uint>("xmin")
                   .HasColumnName("xmin")
                   .IsRowVersion();

            builder.Property<NpgsqlRange<DateTimeOffset>>("slot")
                   .HasColumnName("slot")
                   .HasColumnType("tstzrange")
                   .HasComputedColumnSql("tstzrange(start_utc, end_utc, '[)')", stored: true);
        }
    }
}

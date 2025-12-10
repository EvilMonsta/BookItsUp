using BookItsUp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookItsUp.DataAccess.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.ToTable("customers");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Email)
                   .HasMaxLength(256);

            builder.Property(x => x.Phone)
                   .HasMaxLength(32);

            builder.HasIndex(x => new { x.OrganizationId, x.Email });
            builder.HasIndex(x => new { x.OrganizationId, x.Phone });

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

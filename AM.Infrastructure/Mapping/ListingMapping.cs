using System.Security.Cryptography.Xml;
using AM.Domain.ListingAggregate;
using AM.Domain.NaceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class ListingMapping : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.ToTable("Listing", schema: "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Unit).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50); ;
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.Image).IsRequired().HasMaxLength(64);
            builder.Property(x => x.DeliveryMethod).IsRequired().HasMaxLength(50);
            builder.Property(x => x.DeliveryLocationId).IsRequired();
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(10);

            builder.HasOne(x => x.User).WithMany(x => x.Listings).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.PurchaseList).WithOne(x => x.Listing).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.NegotiateList).WithOne(x => x.Listing).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.SupplyList).WithOne(x => x.Listing).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.DealList).WithOne(x => x.Listing).HasForeignKey(x => x.ListingId);

            builder.OwnsMany(x => x.ListingOperations, ModelBuilder =>
            {
                ModelBuilder.ToTable("ListingOperation", schema: "dbo");
                ModelBuilder.HasKey(x => x.Id);
                ModelBuilder.WithOwner(x => x.Listing).HasForeignKey(x => x.ListingId);
                ModelBuilder.Property(x => x.UserId).IsRequired();
                ModelBuilder.Property(x => x.Description).HasMaxLength(300);

            });
        }
    }
}
using AM.Domain.ListingAggregate;
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
            builder.Property(x => x.Amount).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Unit).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Image).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.DeliveryMethod).IsRequired().HasMaxLength(50);

            builder.HasOne(x => x.User).WithMany(x => x.Listings).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.PurchaseList).WithOne(x => x.Listing).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.SupplyList).WithOne(x => x.Listing).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.DealList).WithOne(x => x.Listing).HasForeignKey(x => x.ListingId);
        }
    }
}
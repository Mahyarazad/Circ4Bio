using AM.Domain.Supplied.PurchasedAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class PurchasedItemMapping : IEntityTypeConfiguration<PurchasedItem>
    {
        public void Configure(EntityTypeBuilder<PurchasedItem> builder)
        {
            builder.ToTable("PurchasedItem", schema: "dbo");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Listing).WithMany(x => x.PurchaseList).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.Users).WithOne(x => x.PurchasedItem).HasForeignKey(x => x.PurchasedItemId);
        }
    }
}
using AM.Domain.Supplied.PurchasedAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class SuppliedItemMapping : IEntityTypeConfiguration<SuppliedItem>
    {
        public void Configure(EntityTypeBuilder<SuppliedItem> builder)
        {
            builder.ToTable("SuppliedItem", schema: "dbo");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Listing).WithMany(x => x.SupplyList).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.Users).WithOne(x => x.SuppliedItem).HasForeignKey(x => x.SuppliedItemId);
        }
    }
}
using AM.Domain.Supplied.PurchasedAggregate;
using AM.Domain.UserAggregate;
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
            builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.TotalPaid).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.HasOne(x => x.Listing).WithMany(x => x.PurchaseList).HasForeignKey(x => x.ListingId);
        }
    }
}
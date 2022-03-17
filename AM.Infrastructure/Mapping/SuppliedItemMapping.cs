using AM.Domain.Supplied.PurchasedAggregate;
using AM.Domain.UserAggregate;
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
            builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.TotalPaid).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();

            builder.HasOne(x => x.Listing).WithMany(x => x.SupplyList).HasForeignKey(x => x.ListingId);
        }
    }
}
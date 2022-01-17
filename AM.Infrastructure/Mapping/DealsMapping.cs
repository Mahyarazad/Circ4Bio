using AM.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class DealsMapping : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.ToTable("Deals", schema: "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ContractFile).IsRequired(false);
            builder.Property(x => x.TrackingCode).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.ClosingStatus).IsRequired();
            builder.Property(x => x.PaymentStatus).IsRequired();


            builder.HasOne(x => x.Listing).WithMany(x => x.DealList).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.Users).WithOne(x => x.Deal).HasForeignKey(x => x.DealId);
        }
    }
}
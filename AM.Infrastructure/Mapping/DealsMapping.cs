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
            builder.Property(x => x.DeliveryCost).IsRequired();
            builder.Property(x => x.TotalCost).IsRequired();
            builder.Property(x => x.DeliveryMethod).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(4);
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.SellerId).IsRequired();
            builder.Property(x => x.BuyerId).IsRequired();
            builder.Property(x => x.PaymentStatus).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsFinished).IsRequired();
            builder.Property(x => x.DeliveryLocationId).IsRequired();
            builder.OwnsOne(x => x.PaymentInfo, ModelBuilder =>
            {
                ModelBuilder.Property(x => x.PaymentId).HasColumnName(nameof(PaymentInfo.PaymentId)).HasMaxLength(100);
                ModelBuilder.Property(x => x.PayerEmail).HasColumnName(nameof(PaymentInfo.PayerEmail)).HasMaxLength(100);
                ModelBuilder.Property(x => x.PayerFirstName).HasColumnName(nameof(PaymentInfo.PayerFirstName)).HasMaxLength(100);
                ModelBuilder.Property(x => x.PayerLastName).HasColumnName(nameof(PaymentInfo.PayerLastName)).HasMaxLength(100);
                ModelBuilder.Property(x => x.PaymentId).HasColumnName(nameof(PaymentInfo.PaymentId)).HasMaxLength(100);
                ModelBuilder.Property(x => x.PaymentTime).HasColumnName(nameof(PaymentInfo.PaymentTime)).HasMaxLength(100);

            });
            builder.HasOne(x => x.Listing).WithMany(x => x.DealList).HasForeignKey(x => x.ListingId);
            builder.HasMany(x => x.Negotiates).WithOne(x => x.Deal).HasForeignKey(x => x.DealId);
        }
    }
}
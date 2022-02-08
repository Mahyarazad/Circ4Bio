using AM.Domain.NegotiateAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class UserNegotiateMapping : IEntityTypeConfiguration<UserNegotiate>
    {
        public void Configure(EntityTypeBuilder<UserNegotiate> builder)
        {
            builder.ToTable("UserNegotiate", schema: "dbo");
            builder.HasKey(x => new { x.UserId, x.NegotiateId });
            builder.Property(x => x.BuyerBool);

            builder.HasOne(x => x.User).WithMany(x => x.UserNegotiate).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Negotiate).WithMany(x => x.UserNegotiate).HasForeignKey(x => x.NegotiateId);
        }
    }
}
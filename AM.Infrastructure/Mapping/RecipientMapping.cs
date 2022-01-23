using AM.Domain.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class RecipientMapping : IEntityTypeConfiguration<Recipient>
    {
        public void Configure(EntityTypeBuilder<Recipient> builder)
        {
            builder.ToTable("Recipient", schema: "dbo");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Notification).WithMany(x => x.Recipient).HasForeignKey(x => x.NotificationId);
        }
    }
}
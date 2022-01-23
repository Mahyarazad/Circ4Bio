using System.Security.Cryptography.Xml;
using AM.Domain.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class NotificationMapping : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification", schema: "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NotificationBody).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.NotificationTitle).IsRequired().HasMaxLength(200);
            builder.HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.Recipient).WithOne(x => x.Notification).HasForeignKey(x => x.NotificationId);
        }
    }
}
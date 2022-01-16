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
            builder.Property(x => x.NotificationTitle).IsRequired().HasMaxLength(50);
            builder.HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserId);

            builder.OwnsMany(x => x.Recipient, ModelBuilder =>
            {
                ModelBuilder.ToTable("Recipient");
                ModelBuilder.HasKey(x => x.Id);
                ModelBuilder.Property(x => x.UserId);
                ModelBuilder.Property(x => x.RoleId);
            });
        }
    }
}
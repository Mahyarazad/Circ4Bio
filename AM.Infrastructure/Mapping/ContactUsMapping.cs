using AM.Domain.ContactUsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class ContactUsMapping : IEntityTypeConfiguration<ContactUs>
    {

        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.ToTable("ContactUs", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Subject).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Phone).IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.Body).IsRequired().HasMaxLength(1500);
        }
    }
}
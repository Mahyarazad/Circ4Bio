using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class ResetPasswordMapping : IEntityTypeConfiguration<ResetPassword>
    {

        public void Configure(EntityTypeBuilder<ResetPassword> builder)
        {
            builder.ToTable("ResetPassword", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ResetUrl).IsRequired().HasMaxLength(32);
        }
    }
}
using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");
            builder.HasOne(x => x.Role).WithMany(x => x.Accounts).HasForeignKey(x => x.RoleId);
        }
    }
}
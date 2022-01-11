using System.Security.Cryptography.X509Certificates;
using AM.Domain;
using AM.Domain.RoleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", schema: "dbo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasMany(x => x.Users).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);

            builder.OwnsMany(x => x.Permissions, NavigationBuilder =>
            {
                NavigationBuilder.HasKey(x => x.Id);
                NavigationBuilder.Ignore(x => x.Name);
                NavigationBuilder.ToTable("RolePermissions");
                NavigationBuilder.WithOwner(x => x.Role);
            });
        }
    }
}
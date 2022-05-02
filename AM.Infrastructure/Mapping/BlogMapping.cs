using System;
using AM.Domain.BlogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class BlogMapping : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blog", schema: "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Auther).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Slug).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.AvatarImage).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.Category).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ReadDuration).IsRequired();
            builder.Property(x => x.ShortDescription).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Body).IsRequired().HasMaxLength(Int32.MaxValue);
            builder.Property(x => x.Image).IsRequired(false).HasMaxLength(200);

            builder.HasOne(x => x.User).WithMany(x => x.Blogs).HasForeignKey(x => x.UserId);
        }
    }
}
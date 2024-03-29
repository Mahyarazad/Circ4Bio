﻿using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(100).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ActivationGuid).HasMaxLength(36).IsRequired();
            builder.Property(x => x.Avatar).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.Address).HasMaxLength(300).IsRequired(false);
            builder.Property(x => x.Country).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.CompanyName).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.City).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.PostalCode).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.VatNumber).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.FaceBookUrl).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.LinkdinUrl).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.InstagramUrl).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.WebUrl).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.TwitterUrl).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired(false);

            builder.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);
            // builder.HasOne(x => x.PurchasedItem).WithOne(x => x.Users).HasForeignKey<PurchasedItem>(x => x.UserId);
            // builder.HasOne(x => x.SuppliedItem).WithOne(x => x.Users).HasForeignKey<SuppliedItem>(x => x.UserId);
            builder.HasMany(x => x.Notifications).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.Listings).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.Blogs).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            builder.OwnsMany(x => x.DeliveryLocations, ModelBuilder =>
            {
                ModelBuilder.HasKey(x => x.Id);
                ModelBuilder.Property(x => x.Name).IsRequired().HasMaxLength(20);
                ModelBuilder.Property(x => x.AddressLineOne).IsRequired().HasMaxLength(100);
                ModelBuilder.Property(x => x.AddressLineTwo).HasMaxLength(100);
                ModelBuilder.Property(x => x.Country).IsRequired().HasMaxLength(30);
                ModelBuilder.Property(x => x.City).IsRequired().HasMaxLength(30);
                ModelBuilder.WithOwner(x => x.User).HasForeignKey(x => x.UserId);

            });
        }
    }
}
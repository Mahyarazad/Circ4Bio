﻿using AM.Domain;
using AM.Domain.BlogAggregate;
using AM.Domain.ContactUsAggregate;
using AM.Domain.ListingAggregate;
using AM.Domain.NaceAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.NotificationAggregate;
using AM.Domain.ResetPasswordAggregate;
using AM.Domain.RoleAggregate;
using AM.Domain.Supplied.PurchasedAggregate;
using AM.Domain.UserAggregate;
using AM.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure
{
    public class AMContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ResetPassword> UserResetPasswords { get; set; }
        public DbSet<ContactUs> ContactUsMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<SuppliedItem> SuppliedItems { get; set; }
        public DbSet<PurchasedItem> PurchasedItems { get; set; }
        public DbSet<Negotiate> Negotiates { get; set; }
        public DbSet<UserNegotiate> UserNegotiates { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Nace> Naces { get; set; }
        public DbSet<Detail> NaceDetails { get; set; }
        public DbSet<ListItems> NaceListItemsDetail { get; set; }
        public DbSet<NaceData> NaceDatas { get; set; }
        public AMContext(DbContextOptions<AMContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(UserMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
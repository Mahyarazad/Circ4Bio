using AM.Domain.ContactUsAggregate;
using AM.Domain.NotificationAggregate;
using AM.Domain.ResetPasswordAggregate;
using AM.Domain.RoleAggregate;
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
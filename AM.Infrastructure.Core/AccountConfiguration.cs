using _0_Framework.Infrastructure;
using AM.Application;
using AM.Application.Contracts.Role;
using AM.Application.Contracts.User;
using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;
using Microsoft.Extensions.DependencyInjection;
using AM.Infrastructure;
using AM.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Core
{
    public class AccountConfiguration
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IUserApplication, UserApplication>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IRoleApplication, RoleApplication>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IPermissionExposer, UserPermissionExposer>();

            services.AddDbContext<AMContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
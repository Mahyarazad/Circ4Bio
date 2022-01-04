using Microsoft.Extensions.DependencyInjection;
using AM.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Core
{
    public class AMConfiguration
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AMContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
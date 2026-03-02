using CRUD_Operation.Data;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operation.Services.Extensions
{
    public static class DbContextServiceExtensions
    {
        public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not set in appsettings.json.");

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}

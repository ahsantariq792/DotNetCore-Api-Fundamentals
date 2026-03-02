using CRUD_Operation.Repositories.Implementations;
using CRUD_Operation.Repositories.Interfaces;
using CRUD_Operation.Services.Implementations;
using CRUD_Operation.Services.Interfaces;

namespace CRUD_Operation.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextServices(configuration);

            services.AddScoped<ISuperHeroRepository, SuperHeroRepository>();
            services.AddScoped<ISuperHeroService, SuperHeroService>();

            return services;
        }
    }
}
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
            services.AddHttpContextAccessor();
            services.AddScoped<CRUD_Operation.Core.Interfaces.ICurrentRequestSpecification, CRUD_Operation.Core.Utilities.CurrentRequestSpecification>();

            services.AddDbContextServices(configuration);
            services.AddJwtAuthentication(configuration);

            services.AddScoped<ISuperHeroRepository, SuperHeroRepository>();
            services.AddScoped<ISuperHeroService, SuperHeroService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
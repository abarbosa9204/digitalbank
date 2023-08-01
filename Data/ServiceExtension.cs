using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class ServiceExtension
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
        }
    }
}
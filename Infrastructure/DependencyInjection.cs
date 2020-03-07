using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IAppDatabase, AppDatabase>();
            //services.AddSingleton<IGenreRepository, GenreRepository>();
            //services.AddSingleton<IPullGenresAppService, PullGenresAppService>();

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}

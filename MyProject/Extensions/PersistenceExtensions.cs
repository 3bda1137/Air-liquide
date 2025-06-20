using Microsoft.EntityFrameworkCore;
using MyProject.Infrastructures.DbContexts;
using MyProject.Infrastructures.Repositories;

namespace MyProject.Extensions
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection ConfigurePersistence(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

                if (environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(); 
                    options.EnableDetailedErrors();
                    options.LogTo(Console.WriteLine); 
                }

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}

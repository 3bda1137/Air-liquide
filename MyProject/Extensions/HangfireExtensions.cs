using Hangfire;
using Hangfire.SqlServer;
using MyProject.Configrations.Hangfire;

namespace MyProject.Extensions
{
    public static class HangfireExtensions
    {
        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var hangfireSettings = configuration.GetSection("HangfireSettings").Get<HangfireSettings>();

            if (hangfireSettings?.Enabled == true)
            {
                var hangfireConnectionString = hangfireSettings.ConnectionString;
                Console.WriteLine($"HangfireConnectionString : {hangfireConnectionString}");
                if (string.IsNullOrEmpty(hangfireConnectionString))
                {
                    throw new Exception("Hangfire: Connection string is missing. Ensure it's set in appsettings.json.");
                }

                UseSqlServer(hangfireConnectionString);
                services.AddHangfireServer();
            }

            void UseSqlServer(string hangfireConnectionString)
            {
                services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
                    {
                        SchemaName = "Hangfire",
                        QueuePollInterval = TimeSpan.Zero,
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            }

        
        }

        public static void UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            var hangfireSettings = configuration.GetSection("HangfireSettings").Get<HangfireSettings>();

            if (hangfireSettings?.Enabled == true)
            {
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new HangfireDashboardAuthFilter("admin", "Abdallah1234@mo") }
                });
            }

        }
    }
}

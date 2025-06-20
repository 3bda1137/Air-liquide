
using Newtonsoft.Json;

namespace MyProject.Extensions
{
    public static class AppSettingsExtensions
    {
        public static void ReadAppSettings(this WebApplicationBuilder webApplicationBuilder)
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "Configrations");

            webApplicationBuilder.Configuration
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{webApplicationBuilder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"Hangfire/appsettings.hangfire.{webApplicationBuilder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"DataBases/appsettings.database.{webApplicationBuilder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)

                .AddEnvironmentVariables();
            var configuration = webApplicationBuilder.Configuration;
        }
    }
}

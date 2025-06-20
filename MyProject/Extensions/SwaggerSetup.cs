using System.Reflection;
using Microsoft.AspNetCore.Builder;
using MyProject.Filters;

namespace MyProject.Extensions
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MyProject API",
                    Version = "v1"
                });
                c.OperationFilter<SwaggerHeader>();

                // Modified XML documentation handling
                try
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: XML documentation file not found at {xmlPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Could not load XML documentation: {ex.Message}");
                }
            });
        }
        public static void UseCustomSwaggerConfig(this IApplicationBuilder app)
        {
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProject.API v1");
                c.RoutePrefix = string.Empty;
            });
        }


    }

}

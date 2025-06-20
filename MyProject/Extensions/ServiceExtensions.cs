namespace MyProject.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = null; // Keeps PascalCase
            });
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


            return services;
        }

        //public static IServiceCollection AddMapster(this IServiceCollection services)
        //{
        //    var config = TypeAdapterConfig.GlobalSettings;
        //    MappingConfig.RegisterMappings(config);
        //    services.AddSingleton(config);
        //    services.AddScoped<IMapper, ServiceMapper>();
        //    return services;
        //}
    }
}

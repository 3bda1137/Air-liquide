using MyProject.Extensions;
using MyProject.Shared.Models;
using MyProject.EndPoints;
using MyProject.Middlewares;

namespace MyProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("------------------Starting up-----------------");
            var builder = WebApplication.CreateBuilder(args);

            builder.ReadAppSettings();
            builder.Services.AddControllers();

            // Add Swagger configuration
            builder.Services.AddSwaggerConfig();  // Your custom Swagger setup

            //builder.Services.ConfigureHangfire(builder.Configuration);
            builder.Services.ConfigurePersistence(builder.Configuration, builder.Environment);
            builder.Services.ConfigureCors(builder.Configuration);

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

            builder.Services.AddScoped<RequestHandlerBaseParameters>();
            builder.Services.AddScoped(typeof(EndpointBaseParameters<>));
            builder.Services.AddScoped<TransactionMiddleware>();
            builder.Services.AddCap(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                x.UseRabbitMQ(options =>
                {
                    options.HostName = "localhost"; // Replace with your RabbitMQ host
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCustomSwaggerConfig();  
            }

            //app.UseHangfireDashboard(app.Configuration);
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<TransactionMiddleware>();

            app.Run();

            Console.WriteLine("----------------Started Successfully-----------------------------");
        }
    }
}
using Bimeister.Habr.TestWebApp.Repository;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace Bimeister.Habr.TestWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables()
                .Build();

                var port = configuration.GetValue<int>("VitePortNumber", 4444);
                builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
                builder.Services.AddAuthorization();
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                        builder.SetIsOriginAllowed(_ => true)
                            .WithOrigins($"http://localhost:{port}")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });
                // Add services to the container.
                builder.Services.AddControllers()
                    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddHttpContextAccessor();
                //DI
                builder.Services.AddScoped<IProductRepository, ProductRepository>();
                if (builder.Environment.IsStaging() || builder.Environment.IsProduction())
                {
                    builder.Services.AddSpaStaticFiles(configuration =>
                    {
                        configuration.RootPath = "ClientApp/dist";
                    });
                }
                //
                var app = builder.Build();
                app.UseStaticFiles();
                if (builder.Environment.IsStaging() || builder.Environment.IsProduction())
                {
                    app.UseSpaStaticFiles();
                }
                else
                {
                    app.UseCors();
                }
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints => endpoints.MapControllers());
                app.UseSpa(spa =>
                {
                    if (app.Environment.IsDevelopment())
                    {
                        spa.UseProxyToSpaDevelopmentServer($"http://localhost:{port}");
                    }
                });
                app.Run();
        }
    }
}

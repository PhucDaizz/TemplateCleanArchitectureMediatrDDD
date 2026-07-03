using AuthService.API.Extensions;
using AuthService.API.StartUp;
using AuthService.Application;
using AuthService.Infrastructure;

namespace AuthService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCustomExceptionHandling();

            builder.Services.AddHealthChecks();

            builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

            builder.AddDependencies();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            var app = builder.Build();

            app.UseExceptionHandler();

            app.UseSwaggerConfiguration();

            app.UseHttpsRedirection();

            app.MapHealthChecks("/health"); 

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

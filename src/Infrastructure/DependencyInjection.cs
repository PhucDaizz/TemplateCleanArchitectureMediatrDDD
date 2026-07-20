using AuthService.Application.Common.Interfaces;
using AuthService.Infrastructure.Services;
using AuthService.Infrastructure.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(
                   configuration.GetConnectionString("DefaultConnection"),
                   new MySqlServerVersion(new Version(8, 0, 21)),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            var rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQSetting>();
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumers(typeof(DependencyInjection).Assembly);
                cfg.UsingRabbitMq((context, rabbitCfg) =>
                {
                    rabbitCfg.Host(rabbitMQConfig!.HostName, "/", h => {
                        h.Username(rabbitMQConfig.UserName);
                        h.Password(rabbitMQConfig.Password);
                    });

                    rabbitCfg.AutoDelete = false;
                    rabbitCfg.Durable = true;    

                    rabbitCfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddScoped<IIntegrationEventService, IntegrationEventService>();
            return services;
        }
    }
}

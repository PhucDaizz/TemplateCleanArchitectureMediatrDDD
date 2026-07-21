using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Repositories;
using AuthService.Infrastructure.Data.Repositories;
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
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

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

            services.AddScoped<ISubcriberRepository, SubcriberRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();

            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddScoped<IIntegrationEventService, IntegrationEventService>();
            return services;
        }
    }
}

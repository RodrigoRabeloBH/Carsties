using BiddingService.Application.Services;
using BiddingService.Domain.Interfaces.Repositories;
using BiddingService.Domain.Interfaces.Services;
using BiddingService.Infrastructure.Consumers;
using BiddingService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BiddingService.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static IServiceCollection AddDependencyResolver(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterMassTransit(services, configuration);
            RegisterServices(services);
            RegisterRepositories(services);

            services.AddHostedService<CheckAuctionFinishedService>();

            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBidServices, BidServices>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IBidRepository, BidRepository>();
        }

        private static void RegisterMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            string user = configuration["RABBIT_USER"] ?? "guest";

            string password = configuration["RABBIT_PASSWORD"] ?? "guest";

            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["RABBIT_HOST"], "/", host =>
                    {
                        host.Username(user);
                        host.Password(password);
                    });

                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}

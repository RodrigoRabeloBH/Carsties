using AuctionSearchService.Application;
using AuctionSearchService.Domain.Interfaces.Repositories;
using AuctionSearchService.Domain.Interfaces.Services;
using AuctionSearchService.Infrastructure.Consumers;
using AuctionSearchService.Infrastructure.Data;
using AuctionSearchService.Infrastructure.Http;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace AuctionSearchService.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependecyResolver
    {
        public static IServiceCollection AddDependecyResolver(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterRepositories(services);
            RegisterServices(services);
            RegisterHttp(services, configuration);
            RegisterMassTransit(services);

            return services;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IItemRepository, ItemRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ISearchServices, SearchServices>();
            services.AddScoped<IAuctionSearchServiceHttp, AuctionSearchServiceHttp>();
        }

        private static void RegisterHttp(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(nameof(AuctionSearchServiceHttp), configureClient =>
            {
                configureClient.BaseAddress = new Uri(configuration["AUCTION_SERVICE_URL"]);
            }).AddPolicyHandler(GetPolicy());
        }

        private static void RegisterMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
                x.UsingRabbitMq((context, config) =>
                {
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2));
        }
    }
}

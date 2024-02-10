﻿using AuctionService.Application.Services;
using AuctionService.Domain.Interfaces.Repository;
using AuctionService.Domain.Interfaces.Services;
using AuctionService.Infrastructure.Consumers;
using AuctionService.Infrastructure.Data;
using AuctionService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AuctionService.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static IServiceCollection AddDependencyResolver(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterRepositories(services);
            RegisterServices(services);
            RegisterDatabases(services, configuration);
            RegisterMassTransit(services);

            return services;
        }

        private static void RegisterDatabases(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuctionDbContext>(opt =>
            {
                opt.UseNpgsql(configuration["AUCTION_DB"]);
            });
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IAuctionRepository, AuctionRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuctionServices, AuctionServices>();
        }

        private static void RegisterMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

                x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(10);
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                x.UsingRabbitMq((context, config) =>
                {
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}

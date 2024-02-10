using AuctionSearchService.Domain.Interfaces.Services;
using AuctionSearchService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Infrastructure.Data
{
    [ExcludeFromCodeCoverage]
    public class DbInitializer
    {
        public static async Task InitDb(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var http = scope.ServiceProvider.GetService<IAuctionSearchServiceHttp>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>();

            try
            {
                await DB.InitAsync(configuration["DATABASE"], MongoClientSettings.FromConnectionString(configuration["MONGODB_CONNECTION"]));

                await DB.Index<Item>()
                    .Key(x => x.Make, KeyType.Text)
                    .Key(x => x.Model, KeyType.Text)
                    .Key(x => x.Color, KeyType.Text)
                    .CreateAsync();

                var count = await DB.CountAsync<Item>();

                var items = await http.GetItemsForSearchDb();

                logger.LogInformation("[DbInitializer].[InitDb] - Returned from the auction service");

                if (items.Count > 0)
                    await DB.SaveAsync(items);

            }
            catch (Exception ex)
            {
                logger.LogError($"[DB INITIALIZER]-[INIT DB] Error: {ex.Message}", ex);
            }
        }
    }
}

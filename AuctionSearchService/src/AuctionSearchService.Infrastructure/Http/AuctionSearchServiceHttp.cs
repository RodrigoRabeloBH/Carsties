using AuctionSearchService.Domain.Interfaces.Services;
using AuctionSearchService.Domain.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace AuctionSearchService.Infrastructure.Http
{
    [ExcludeFromCodeCoverage]
    public class AuctionSearchServiceHttp : IAuctionSearchServiceHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuctionSearchServiceHttp> _logger;
        private const string ENDPOINT = "/api/auctions";

        public AuctionSearchServiceHttp(IHttpClientFactory httpClientFactory, ILogger<AuctionSearchServiceHttp> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<Item>> GetItemsForSearchDb()
        {
            try
            {
                _logger.LogInformation("[Auction SerachService Http].[Get Items For SearchDb] - Getting items from Auction Service");

                var lastUpdated = await DB.Find<Item, string>()
                    .Sort(x => x.Descending(x => x.UpdateAt))
                    .Project(x => x.UpdateAt.ToString())
                    .ExecuteFirstAsync();

                var http = _httpClientFactory.CreateClient(nameof(AuctionSearchServiceHttp));

                var items = await http.GetFromJsonAsync<List<Item>>(ENDPOINT + "?date=" + lastUpdated);

                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Auction SerachService Http].[Get Items For SearchDb] Error message: {ex.Message}", ex);

                throw;
            }
        }
    }
}

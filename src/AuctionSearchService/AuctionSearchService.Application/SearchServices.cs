using AuctionSearchService.Domain.Interfaces.Repositories;
using AuctionSearchService.Domain.Interfaces.Services;
using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AuctionSearchService.Application
{
    public class SearchServices : ISearchServices
    {
        private readonly ILogger<SearchServices> _logger;
        private readonly IItemRepository _rep;

        public SearchServices(ILogger<SearchServices> logger, IItemRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }

        public async Task<ItemsResult> GetAllItems(SearchParams searchParams)
        {
            _logger.LogInformation($"[Search Services].[Get All Items] - Getting all items with search term like: {JsonSerializer.Serialize(searchParams)}");

            return await _rep.GetAll(searchParams);
        }
    }
}

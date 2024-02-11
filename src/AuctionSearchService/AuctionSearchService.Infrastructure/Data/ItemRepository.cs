using AuctionSearchService.Domain.Interfaces.Repositories;
using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Infrastructure.Data
{
    [ExcludeFromCodeCoverage]
    public class ItemRepository : IItemRepository
    {
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(ILogger<ItemRepository> logger)
        {
            _logger = logger;
        }

        public async Task<ItemsResult> GetAll(SearchParams searchParams)
        {
            try
            {
                var query = DB.PagedSearch<Item, Item>();

                query = AddFilters(searchParams, query);

                var (Results, TotalCount, PageCount) = await query.ExecuteAsync();

                return new ItemsResult { PageCount = PageCount, Results = Results, TotalCount = TotalCount };

            }
            catch (Exception ex)
            {
                _logger.LogError($"[ITEM REPOSITORY].[GET ALL ITEMS] - Error message: {ex.Message}", ex);

                throw;
            }
        }

        private PagedSearch<Item, Item> AddFilters(SearchParams searchParams, PagedSearch<Item, Item> query)
        {
            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);

            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();

            if (!string.IsNullOrEmpty(searchParams.Seller))
                query.Match(x => x.Seller.ToLower() == searchParams.Seller.ToLower());

            if (!string.IsNullOrEmpty(searchParams.Winner))
                query.Match(x => x.Winner.ToLower() == searchParams.Winner.ToLower());

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(s => s.Ascending(i => i.Make)),
                "new" => query.Sort(s => s.Descending(i => i.CreateAt)),
                _ => query.Sort(s => s.Ascending(i => i.AuctionEnd))
            };

            query = searchParams.FilterBy switch
            {
                "finished" => query.Match(i => i.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(i => i.AuctionEnd < DateTime.UtcNow.AddHours(6) && i.AuctionEnd > DateTime.UtcNow),
                _ => query.Match(i => i.AuctionEnd > DateTime.UtcNow)
            };

            return query;
        }
    }
}

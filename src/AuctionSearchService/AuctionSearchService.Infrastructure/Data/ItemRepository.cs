using AuctionContracts;
using AuctionSearchService.Domain.Interfaces.Repositories;
using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;
using MassTransit;
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

        public async Task<Item> GetById(string auctionId)
        {
            return await DB.Find<Item>().OneAsync(auctionId);
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
                _logger.LogError(ex, "[ITEM REPOSITORY].[GET ALL ITEMS] - Error message: {error}", ex.Message);

                throw;
            }
        }

        public async Task SaveAsync(Item item)
        {
            try
            {
                await DB.SaveAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ITEM REPOSITORY] --> Error message: {error}", ex.Message);

                throw;
            }
        }

        public async Task UpdateAsync(Item item, string auctionId)
        {
            try
            {
                var result = await DB.Update<Item>()
                                     .Match(a => a.ID == auctionId)
                                     .ModifyOnly(i => new
                                     {
                                         i.Color,
                                         i.Make,
                                         i.Model,
                                         i.Year,
                                         i.Mileage
                                     }, item)
                                     .ExecuteAsync();

                if (!result.IsAcknowledged)
                    throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ITEM REPOSITORY] --> Error message: {error}", ex.Message);

                throw;
            }
        }

        public async Task DeleteAsync(string auctionId)
        {
            try
            {
                var result = await DB.DeleteAsync<Item>(auctionId);

                if (!result.IsAcknowledged)
                    throw new MessageException(typeof(AuctionDeleted), "Problem deleting message from mongodb");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ITEM REPOSITORY] --> Error message: {error}", ex.Message);

                throw;
            }
        }

        private static PagedSearch<Item, Item> AddFilters(SearchParams searchParams, PagedSearch<Item, Item> query)
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
                "make" => query.Sort(s => s.Ascending(i => i.Make)).Sort(x => x.Ascending(a => a.Model)),
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

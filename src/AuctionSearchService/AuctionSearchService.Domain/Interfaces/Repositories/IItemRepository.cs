using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;

namespace AuctionSearchService.Domain.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task<Item> GetById(string auctionId);
        Task<ItemsResult> GetAll(SearchParams searchParamspageSize);
        Task DeleteAsync(string auctionId);
        Task SaveAsync(Item item);
        Task UpdateAsync(Item item, string auctionId);
    }
}

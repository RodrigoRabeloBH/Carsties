using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;

namespace AuctionSearchService.Domain.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task<ItemsResult> GetAll(SearchParams searchParamspageSize);
    }
}

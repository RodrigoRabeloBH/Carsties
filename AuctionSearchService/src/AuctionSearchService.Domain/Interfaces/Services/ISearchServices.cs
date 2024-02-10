using AuctionSearchService.Domain.Models;
using AuctionSearchService.Domain.RequestHelper;

namespace AuctionSearchService.Domain.Interfaces.Services
{
    public interface ISearchServices
    {
        Task<ItemsResult> GetAllItems(SearchParams searchParams);
    }
}

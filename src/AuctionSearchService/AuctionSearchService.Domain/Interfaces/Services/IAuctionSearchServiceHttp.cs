using AuctionSearchService.Domain.Models;

namespace AuctionSearchService.Domain.Interfaces.Services
{
    public interface IAuctionSearchServiceHttp
    {
        Task<List<Item>> GetItemsForSearchDb();
    }
}

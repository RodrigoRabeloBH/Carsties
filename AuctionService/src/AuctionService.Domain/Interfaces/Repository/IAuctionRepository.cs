using AuctionService.Domain.Entities;

namespace AuctionService.Domain.Interfaces.Repository
{
    public interface IAuctionRepository : IBaseRepository<Auction>
    {
        Task<List<Auction>> GetAllAuction(string date);

        Task<Auction> GetAuctionById(Guid id);
    }
}

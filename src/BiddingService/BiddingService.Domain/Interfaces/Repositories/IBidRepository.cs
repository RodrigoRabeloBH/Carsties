using BiddingService.Domain.Models;

namespace BiddingService.Domain.Interfaces.Repositories
{
    public interface IBidRepository
    {
        Task<List<Bid>> GetBids(string auctionId);
        Task<Auction> GetAuctionById(string auctionId);
        Task<Bid> GetHighBid(string auctionId);
        Task Create(Bid bid);
    }
}

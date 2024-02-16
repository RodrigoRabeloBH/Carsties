using BiddingService.Domain.DTOs;
using BiddingService.Domain.Models;

namespace BiddingService.Domain.Interfaces.Services
{
    public interface IBidServices
    {
        Task<List<BidDto>> GetAllBidsByAuctionId(string auctionId);
        Task<Auction> GetAuction(string auctionId);
        Task<BidDto> CreateBid(Bid bid, string auctionId, Auction auction);
    }
}

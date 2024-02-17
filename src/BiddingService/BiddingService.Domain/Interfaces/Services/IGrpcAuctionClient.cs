using BiddingService.Domain.Models;

namespace BiddingService.Domain.Interfaces.Services
{
    public interface IGrpcAuctionClient
    {
        Auction GetAuction(string id);
    }
}

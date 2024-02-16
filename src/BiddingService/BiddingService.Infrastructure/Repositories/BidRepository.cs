using BiddingService.Domain.Interfaces.Repositories;
using BiddingService.Domain.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;

namespace BiddingService.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BidRepository : IBidRepository
    {
        private readonly ILogger<BidRepository> _logger;

        public BidRepository(ILogger<BidRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Auction> GetAuctionById(string auctionId)
        {
            try
            {
                var auction = await DB.Find<Auction>()
                    .OneAsync(auctionId);

                return auction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BID REPOSITORY].[GET AUCTION BY ID] --> Error message: {error}", ex.Message);

                throw;
            }
        }
        public async Task<List<Bid>> GetBids(string auctionId)
        {
            try
            {
                var bids = await DB.Find<Bid>()
                    .Match(b => b.AuctionId == auctionId)
                    .Sort(b => b.Descending(x => x.BidTime))
                    .ExecuteAsync();

                return bids;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BID REPOSITORY].[GET BIDS] --> Error message: {error}", ex.Message);

                throw;
            }
        }

        public async Task<Bid> GetHighBid(string auctionId)
        {
            try
            {
                var bid = await DB.Find<Bid>()
                    .Match(b => b.AuctionId == auctionId)
                    .Sort(b => b.Descending(x => x.Amount))
                    .ExecuteFirstAsync();

                return bid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BID REPOSITORY].[GET HIGH BID] --> Error message: {error}", ex.Message);

                throw;
            }
        }

        public async Task Create(Bid bid)
        {
            try
            {
                await DB.SaveAsync(bid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BID REPOSITORY].[CREATE] --> Error message: {error}", ex.Message);

                throw;
            }
        }
    }
}

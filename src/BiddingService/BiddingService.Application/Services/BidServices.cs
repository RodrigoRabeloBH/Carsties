using AuctionContracts;
using AutoMapper;
using BiddingService.Domain.DTOs;
using BiddingService.Domain.Enums;
using BiddingService.Domain.Interfaces.Repositories;
using BiddingService.Domain.Interfaces.Services;
using BiddingService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BiddingService.Application.Services
{
    public class BidServices : IBidServices
    {
        private readonly ILogger<BidServices> _logger;
        private readonly IBidRepository _rep;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BidServices(ILogger<BidServices> logger, IBidRepository rep, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _rep = rep;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<BidDto>> GetAllBidsByAuctionId(string auctionId)
        {
            _logger.LogInformation("[BID SERVICES] --> Getting all bids with auction id: {id}", auctionId);

            var bids = await _rep.GetBids(auctionId);

            return _mapper.Map<List<BidDto>>(bids);
        }
        public async Task<BidDto> CreateBid(Bid bid, string auctionId, Auction auction)
        {
            try
            {
                _logger.LogInformation("[BID SERVICES] --> Creating a new bid: {bid}", JsonSerializer.Serialize(bid));

                if (auction.AuctionEnd < DateTime.UtcNow)
                    bid.BidStatus = BidStatus.Finished;
                else
                {
                    var highBid = await _rep.GetHighBid(auctionId);

                    if ((highBid != null && bid.Amount > highBid.Amount) || highBid == null)
                        bid.BidStatus = bid.Amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;

                    if (highBid != null && bid.Amount <= highBid.Amount)
                        bid.BidStatus = BidStatus.TooLow;
                }

                await _rep.Create(bid);

                var bidPlaced = _mapper.Map<BidPlaced>(bid);

                await _publishEndpoint.Publish(bidPlaced);

                return _mapper.Map<BidDto>(bid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BID SERVICES] --> Error message: {error}", ex.Message);

                throw;
            }
        }

        public async Task<Auction> GetAuction(string auctionId)
        {
            _logger.LogInformation("[BID SERVICES] --> Getting auction with id: {id}", auctionId);

            var auction = await _rep.GetAuctionById(auctionId);

            return auction;
        }
    }
}

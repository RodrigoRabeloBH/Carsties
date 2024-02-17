using BiddingService.Domain.DTOs;
using BiddingService.Domain.Interfaces.Services;
using BiddingService.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BiddingService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IBidServices _service;
        private readonly IGrpcAuctionClient _grpcClient;

        public BidsController(IBidServices service, IGrpcAuctionClient grpcClient)
        {
            _service = service;
            _grpcClient = grpcClient;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
        {
            var username = User?.Identity?.Name;

            var auction = await _service.GetAuction(auctionId);

            if (auction == null)
            {
                auction = _grpcClient.GetAuction(auctionId);

                if (auction == null)
                    return BadRequest("Cannot accept bids on this auction at this time");
            }


            if (!string.IsNullOrEmpty(username) && auction.Seller == username)
                return BadRequest("You cannot bid on your own auction");

            var bid = new Bid
            {
                AuctionId = auctionId,
                Amount = amount,
                Bidder = username
            };

            var bidDto = await _service.CreateBid(bid, auctionId, auction);

            return Ok(bidDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
        {
            var bids = await _service.GetAllBidsByAuctionId(auctionId);

            if (bids == null || !bids.Any())
                return NotFound();

            return bids;
        }
    }
}
using AuctionService;
using BiddingService.Domain.Interfaces.Services;
using BiddingService.Domain.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace BiddingService.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class GrpcAuctionClient : IGrpcAuctionClient
    {
        private readonly ILogger<GrpcAuctionClient> _logger;
        private readonly IConfiguration _configuration;

        public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Auction GetAuction(string id)
        {
            _logger.LogInformation("[GRPC AUCTION CLIENTE] --> Calling GRPC Service");

            var channel = GrpcChannel.ForAddress(_configuration["GRPC_AUCTION"]);
            var client = new GrpcAuction.GrpcAuctionClient(channel);
            var request = new GetAuctionRequest { Id = id };

            try
            {
                var reply = client.GetAuction(request);

                var auction = new Auction
                {
                    ID = reply.Auction.Id,
                    AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                    Seller = reply.Auction.Seller,
                    ReservePrice = reply.Auction.ReservePrice
                };

                return auction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GRPC AUCTION CLIENT] --> Error message: {error}", ex.Message);

                throw;
            }
        }
    }
}

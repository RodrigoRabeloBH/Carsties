using AuctionService.Domain.Interfaces.Repository;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AuctionService.Application.Services
{
    public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
    {
        private readonly IAuctionRepository _rep;
        private readonly ILogger<GrpcAuctionService> _logger;

        public GrpcAuctionService(IAuctionRepository rep, ILogger<GrpcAuctionService> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("[GET AUCTION] --> Received Grpc request for auction");

                var id = Guid.Parse(request.Id);

                var auction = await _rep.GetById(id) ?? throw new RpcException(new Status(StatusCode.NotFound, "Not Found"));

                var response = new GrpcAuctionResponse
                {
                    Auction = new GrpcAuctionModel
                    {
                        AuctionEnd = auction.AuctionEnd.ToString(),
                        Id = auction.Id.ToString(),
                        ReservePrice = auction.ReservePrice,
                        Seller = auction.Seller
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GRPC AUCTION SERVICE] --> Error message: {error}", ex.Message);

                throw;
            }
        }
    }
}

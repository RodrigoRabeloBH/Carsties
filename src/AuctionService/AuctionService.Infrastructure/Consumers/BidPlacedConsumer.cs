using AuctionContracts;
using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces.Repository;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuctionService.Infrastructure.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly IBaseRepository<Auction> _rep;
        private readonly ILogger<BidPlacedConsumer> _logger;

        public BidPlacedConsumer(IBaseRepository<Auction> rep, ILogger<BidPlacedConsumer> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            try
            {
                Guid id = Guid.Parse(context.Message.AuctionId);

                _logger.LogInformation("[BID PLACED CONSUMER] --> Consuming bid placed with auction id: {id}", id);

                var auction = await _rep.GetById(id);

                if (auction.CurrentHighBid == null ||
                    context.Message.BidStatus.Contains("Accepted") &&
                    context.Message.Amount > auction.CurrentHighBid)
                {
                    auction.CurrentHighBid = context.Message.Amount;

                    await _rep.Update(auction);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BID PLACED CONSUMER] --> Error message: {error}", ex.Message);

                throw;
            }
        }
    }
}

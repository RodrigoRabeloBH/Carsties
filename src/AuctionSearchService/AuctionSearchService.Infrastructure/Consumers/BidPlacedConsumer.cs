using AuctionContracts;
using AuctionSearchService.Domain.Interfaces.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuctionSearchService.Infrastructure.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly ILogger<BidPlacedConsumer> _logger;

        private readonly IItemRepository _rep;

        public BidPlacedConsumer(ILogger<BidPlacedConsumer> logger, IItemRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            try
            {
                var auction = await _rep.GetById(context.Message.AuctionId);

                if (context.Message.BidStatus.Contains("Accepted") && context.Message.Amount > auction.CurrentHighBid)
                {
                    auction.CurrentHighBid = context.Message.Amount;

                    await _rep.SaveAsync(auction);
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

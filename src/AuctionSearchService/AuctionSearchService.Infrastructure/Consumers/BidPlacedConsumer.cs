using AuctionContracts;
using AuctionSearchService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;

namespace AuctionSearchService.Infrastructure.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly ILogger<BidPlacedConsumer> _logger;

        public BidPlacedConsumer(ILogger<BidPlacedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            try
            {
                var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

                if (context.Message.BidStatus.Contains("Accepted") && context.Message.Amount > auction.CurrentHighBid)
                {
                    auction.CurrentHighBid = context.Message.Amount;

                    await auction.SaveAsync();
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

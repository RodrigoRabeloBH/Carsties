using AuctionContracts;
using AuctionSearchService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;

namespace AuctionSearchService.Infrastructure.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly ILogger<AuctionFinishedConsumer> _logger;

        public AuctionFinishedConsumer(ILogger<AuctionFinishedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            try
            {
                var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

                if (context.Message.ItemSold)
                {
                    auction.Winner = context.Message.Winner;
                    auction.SoldAmount = (int)context.Message.Amount;
                }

                auction.Status = "Finished";

                await auction.SaveAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AUCTION FINISHED CONSUMER] --> Error message: {error}", ex.Message);

                throw;
            }
        }
    }
}

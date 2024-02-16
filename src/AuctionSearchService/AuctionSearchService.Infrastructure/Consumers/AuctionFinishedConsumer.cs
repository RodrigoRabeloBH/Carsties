using AuctionContracts;
using AuctionSearchService.Domain.Interfaces.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuctionSearchService.Infrastructure.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly ILogger<AuctionFinishedConsumer> _logger;
        private readonly IItemRepository _rep;

        public AuctionFinishedConsumer(ILogger<AuctionFinishedConsumer> logger, IItemRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            try
            {
                var auction = await _rep.GetById(context.Message.AuctionId);

                if (context.Message.ItemSold)
                {
                    auction.Winner = context.Message.Winner;
                    auction.SoldAmount = (int)context.Message.Amount;
                }

                auction.Status = "Finished";

                await _rep.SaveAsync(auction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AUCTION FINISHED CONSUMER] --> Error message: {error}", ex.Message);

                throw;
            }
        }
    }
}

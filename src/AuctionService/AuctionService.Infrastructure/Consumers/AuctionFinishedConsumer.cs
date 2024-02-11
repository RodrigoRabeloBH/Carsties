using AuctionContracts;
using AuctionService.Domain.Entities;
using AuctionService.Domain.Enums;
using AuctionService.Domain.Interfaces.Repository;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuctionService.Infrastructure.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly IBaseRepository<Auction> _rep;
        private readonly ILogger<AuctionFinishedConsumer> _logger;

        public AuctionFinishedConsumer(IBaseRepository<Auction> rep, ILogger<AuctionFinishedConsumer> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            try
            {
                Guid id = Guid.Parse(context.Message.AuctionId);

                _logger.LogInformation("[AUCTION FINISHED CONSUMER] --> Consuming aution finished with id: {id}", id);

                var auction = await _rep.GetById(id);

                if (context.Message.ItemSold)
                {
                    auction.Winner = context.Message.Winner;
                    auction.SoldAmount = context.Message.Amount;
                }

                auction.Status = auction.SoldAmount > auction.ReservePrice ? Status.Finished : Status.ReserveNotMet;

                await _rep.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AUCTION FINISHED CONSUMER] --> Error message: {error}", ex.Message);

                throw;
            }
        }
    }
}

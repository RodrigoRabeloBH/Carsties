using AuctionContracts;
using AuctionSearchService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Infrastructure.Consumers
{
    [ExcludeFromCodeCoverage]
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly ILogger<AuctionUpdated> _logger;

        public AuctionDeletedConsumer(ILogger<AuctionUpdated> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            try
            {
                _logger.LogInformation("[AUCTION DELETED CONSUMER] --> Consuming auction deleted with id message: {id} ", context.Message.Id);

                var result = await DB.DeleteAsync<Item>(context.Message.Id);

                if (!result.IsAcknowledged)
                    throw new MessageException(typeof(AuctionDeleted), "Problem deleting message from mongodb");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "[AUCTION DELETED CONSUMER] --> Error message: {error} ", ex.Message);

                throw;
            }
        }
    }
}

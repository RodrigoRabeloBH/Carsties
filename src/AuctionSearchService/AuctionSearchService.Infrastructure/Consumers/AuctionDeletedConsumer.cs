using AuctionContracts;
using AuctionSearchService.Domain.Interfaces.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Infrastructure.Consumers
{
    [ExcludeFromCodeCoverage]
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly ILogger<AuctionUpdated> _logger;
        private readonly IItemRepository _rep;

        public AuctionDeletedConsumer(ILogger<AuctionUpdated> logger, IItemRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }

        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            try
            {
                _logger.LogInformation("[AUCTION DELETED CONSUMER] --> Consuming auction deleted with id message: {id} ", context.Message.Id);

                await _rep.DeleteAsync(context.Message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "[AUCTION DELETED CONSUMER] --> Error message: {error} ", ex.Message);

                throw;
            }
        }
    }
}

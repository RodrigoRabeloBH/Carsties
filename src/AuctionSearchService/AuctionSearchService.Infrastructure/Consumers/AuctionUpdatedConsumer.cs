using AuctionContracts;
using AuctionSearchService.Domain.Interfaces.Repositories;
using AuctionSearchService.Domain.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Infrastructure.Consumers
{
    [ExcludeFromCodeCoverage]
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionUpdated> _logger;
        private readonly IItemRepository _rep;

        public AuctionUpdatedConsumer(IMapper mapper, ILogger<AuctionUpdated> logger, IItemRepository rep)
        {
            _mapper = mapper;
            _logger = logger;
            _rep = rep;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            try
            {
                _logger.LogInformation("[AUCTION UPDATED CONSUMER] --> Consuming auction updated with id message: {id} ", context.Message.Id);

                var item = _mapper.Map<Item>(context.Message);

                await _rep.UpdateAsync(item, context.Message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AUCTION UPDATED CONSUMER] --> Error message: {error} ", ex.Message);

                throw;
            }
        }
    }
}

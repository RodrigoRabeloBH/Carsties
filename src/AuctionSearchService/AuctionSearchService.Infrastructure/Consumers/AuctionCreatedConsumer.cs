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
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionCreatedConsumer> _logger;
        private readonly IItemRepository _rep;

        public AuctionCreatedConsumer(IMapper mapper, ILogger<AuctionCreatedConsumer> logger, IItemRepository rep)
        {
            _mapper = mapper;
            _logger = logger;
            _rep = rep;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            try
            {
                _logger.LogInformation("[AUCTION CREATED CONSUMER] --> Consuming auction created with id message: {id} ", context.Message.Id);

                var item = _mapper.Map<Item>(context.Message);

                await _rep.SaveAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Auction Created Consumer].[Consume] - Erro message: {errorMessage}", ex.Message);

                throw;
            }
        }
    }
}

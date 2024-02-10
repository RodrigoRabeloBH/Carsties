using AuctionContracts;
using AuctionSearchService.Domain.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Infrastructure.Consumers
{
    [ExcludeFromCodeCoverage]
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionCreatedConsumer> _logger;

        public AuctionCreatedConsumer(IMapper mapper, ILogger<AuctionCreatedConsumer> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            try
            {
                _logger.LogInformation("[AUCTION CREATED CONSUMER] --> Consuming auction created with id message: {id} ", context.Message.Id);

                var item = _mapper.Map<Item>(context.Message);

                await item.SaveAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Auction Created Consumer].[Consume] - Erro message: {errorMessage}", ex.Message);

                throw;
            }
        }
    }
}

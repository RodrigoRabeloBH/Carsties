using AuctionContracts;
using AuctionSearchService.Domain.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;

namespace AuctionSearchService.Infrastructure.Consumers
{
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
                _logger.LogInformation("[Auction Created Consumer].[Consume] --> Consuming auction created: " + context.Message.Id);

                var item = _mapper.Map<Item>(context.Message);

                await item.SaveAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("[Auction Created Consumer].[Consume] - Erro message: " + ex.Message, ex);
            }
        }
    }
}

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
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionUpdated> _logger;

        public AuctionUpdatedConsumer(IMapper mapper, ILogger<AuctionUpdated> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            try
            {
                _logger.LogInformation("[AUCTION UPDATED CONSUMER] --> Consuming auction updated with id message: {id} ", context.Message.Id);

                var item = _mapper.Map<Item>(context.Message);

                var result = await DB.Update<Item>()
                    .Match(a => a.ID == context.Message.Id)
                    .ModifyOnly(i => new
                    {
                        i.Color,
                        i.Make,
                        i.Model,
                        i.Year,
                        i.Mileage
                    }, item)
                    .ExecuteAsync();

                if (!result.IsAcknowledged)
                    throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AUCTION UPDATED CONSUMER] --> Error message: {error} ", ex.Message);

                throw;
            }
        }
    }
}

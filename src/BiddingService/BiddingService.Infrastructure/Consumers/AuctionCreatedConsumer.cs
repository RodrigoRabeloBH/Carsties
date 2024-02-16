using AuctionContracts;
using BiddingService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;

namespace BiddingService.Infrastructure.Consumers
{
    [ExcludeFromCodeCoverage]
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly ILogger<AuctionCreatedConsumer> _logger;
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            try
            {
                var auction = new Auction
                {
                    ID = context.Message.Id.ToString(),
                    Seller = context.Message.Seller,
                    AuctionEnd = context.Message.AuctionEnd,
                    ReservePrice = context.Message.ReservePrice
                };

                await auction.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AUCTION CREATED CONSUMER] --> Error message: {error}", ex.Message);
            }
        }
    }
}

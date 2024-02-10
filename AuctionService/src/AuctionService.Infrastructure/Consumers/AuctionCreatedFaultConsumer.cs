using AuctionContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuctionService.Infrastructure.Consumers
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        private readonly ILogger<AuctionCreatedFaultConsumer> _logger;

        public AuctionCreatedFaultConsumer(ILogger<AuctionCreatedFaultConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
            _logger.LogInformation("[AUCTION CREATED FAULT CONSUMER] --> Consuming faulty creation");

            var exception = context.Message.Exceptions.First();

            if (exception.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Model = "Default Model";

                await context.Publish(context.Message.Message);
            }
        }
    }
}

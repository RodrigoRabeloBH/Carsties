using AuctionContracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<AuctionCreatedConsumer> _logger;

        public AuctionCreatedConsumer(IHubContext<NotificationHub> hub, ILogger<AuctionCreatedConsumer> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            _logger.LogInformation("[AUCTION CREATED CONSUMER] --> Message received");

            await _hub.Clients.All.SendAsync("AuctionCreated", context.Message);
        }
    }
}

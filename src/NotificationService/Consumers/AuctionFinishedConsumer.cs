using AuctionContracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<AuctionFinishedConsumer> _logger;

        public AuctionFinishedConsumer(IHubContext<NotificationHub> hub, ILogger<AuctionFinishedConsumer> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            _logger.LogInformation("[AUCTION FINISHED CONSUMER] --> Message received");

            await _hub.Clients.All.SendAsync("AuctionFinished", context.Message);
        }
    }
}

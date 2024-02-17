using AuctionContracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<BidPlacedConsumer> _logger;

        public BidPlacedConsumer(IHubContext<NotificationHub> hubContext, ILogger<BidPlacedConsumer> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            _logger.LogInformation("[BID PLACED CONSUMER] --> Message received");

            await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
        }
    }
}

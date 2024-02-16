using AuctionContracts;
using BiddingService.Domain.Enums;
using BiddingService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Entities;
using System.Diagnostics.CodeAnalysis;

namespace BiddingService.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class CheckAuctionFinishedService : BackgroundService
    {
        private readonly ILogger<CheckAuctionFinishedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CheckAuctionFinishedService(ILogger<CheckAuctionFinishedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[CHECK AUCTION FINISHED] --> Starting check for finished auctions");

            stoppingToken.Register(() => _logger.LogInformation(" --> Auction check is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAuctions(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task CheckAuctions(CancellationToken stoppingToken)
        {
            var finishedAuctions = await DB.Find<Auction>()
                 .Match(x => x.AuctionEnd <= DateTime.UtcNow)
                 .Match(x => !x.Finished)
                 .ExecuteAsync(stoppingToken);

            if (finishedAuctions.Count == 0) return;

            _logger.LogInformation("--> Found {count} auctions that have completed", finishedAuctions.Count);

            using var scope = _serviceProvider.CreateScope();

            var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            foreach (var auction in finishedAuctions)
            {
                auction.Finished = true;

                await auction.SaveAsync(null, stoppingToken);

                var winningBid = await DB.Find<Bid>()
                    .Match(x => x.AuctionId == auction.ID)
                    .Match(x => x.BidStatus == BidStatus.Accepted)
                    .Sort(x => x.Descending(y => y.Amount))
                    .ExecuteFirstAsync(stoppingToken);

                var auctionFinished = new AuctionFinished
                {
                    ItemSold = winningBid != null,
                    AuctionId = auction.ID,
                    Winner = winningBid?.Bidder,
                    Amount = winningBid?.Amount,
                    Seller = auction.Seller
                };

                await endpoint.Publish(auctionFinished, stoppingToken);
            }
        }
    }
}

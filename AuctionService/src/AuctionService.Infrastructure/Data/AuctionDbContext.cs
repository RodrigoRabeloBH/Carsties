using AuctionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace AuctionService.Infrastructure.Data
{
    public class AuctionDbContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}

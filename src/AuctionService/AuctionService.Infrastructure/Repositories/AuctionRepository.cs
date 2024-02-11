using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces.Repository;
using AuctionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AuctionService.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AuctionRepository : BaseRepository<Auction>, IAuctionRepository
    {
        public AuctionRepository(ILogger<BaseRepository<Auction>> logger, AuctionDbContext context) : base(logger, context) { }

        public async Task<List<Auction>> GetAllAuction(string date)
        {
            try
            {
                var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

                if (!string.IsNullOrEmpty(date))
                    query = query.Where(x => x.UpdateAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);

                return await query
                    .Include(a => a.Item)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AUCTION REPOSITORY][GET ALL AUCTIONS] - Error: {ex.Message}", ex);

                throw;
            }
        }

        public async Task<Auction> GetAuctionById(Guid id)
        {
            try
            {
                return await _context.Auctions
                    .Include(a => a.Item)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AUCTION REPOSITORY][GET ALL AUCTIONS] - Error: {ex.Message}", ex);

                throw;
            }
        }
    }
}

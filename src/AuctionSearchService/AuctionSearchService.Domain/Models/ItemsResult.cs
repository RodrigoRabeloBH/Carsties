using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class ItemsResult
    {
        public IReadOnlyList<Item> Results { get; set; }
        public int PageCount { get; set; }
        public long TotalCount { get; set; }
    }
}

using System.Diagnostics.CodeAnalysis;

namespace AuctionService.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class AuctionDeleted
    {
        public string Id { get; set; }
    }
}

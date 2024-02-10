using System.Diagnostics.CodeAnalysis;

namespace AuctionService.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        public Guid Id { get; set; }
    }
}

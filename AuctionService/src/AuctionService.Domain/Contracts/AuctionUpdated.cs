﻿using System.Diagnostics.CodeAnalysis;

namespace AuctionService.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class AuctionUpdated
    {
        public string Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public string ImageUrl { get; set; }
    }
}

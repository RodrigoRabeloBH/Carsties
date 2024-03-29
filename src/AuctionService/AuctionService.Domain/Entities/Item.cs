﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AuctionService.Domain.Entities
{
    [Table("Items")]
    [ExcludeFromCodeCoverage]
    public class Item : Entity
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public string ImageUrl { get; set; }

        // nav properties
        public Auction Auction { get; set; }
        public Guid AuctionId { get; set; }
    }
}


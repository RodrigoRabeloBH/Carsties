using AuctionContracts;
using AuctionSearchService.Domain.Models;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace AuctionSearchService.Api.RequestHelper
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuctionCreated, Item>();
            CreateMap<AuctionUpdated, Item>();
        }
    }
}

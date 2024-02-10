using AuctionContracts;
using AuctionSearchService.Domain.Models;
using AutoMapper;

namespace AuctionSearchService.Api.RequestHelper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuctionCreated, Item>();
        }
    }
}

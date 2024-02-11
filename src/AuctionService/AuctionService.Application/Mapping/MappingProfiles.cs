using AuctionContracts;
using AuctionService.Domain.DTOs;
using AuctionService.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace AuctionService.Application.Mapping
{
    [ExcludeFromCodeCoverage]
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction, AuctionDto>()
                .IncludeMembers(x => x.Item);

            CreateMap<Item, AuctionDto>();

            CreateMap<AuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));

            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));

            CreateMap<CreateAuctionDto, Item>();

            CreateMap<AuctionDto, AuctionCreated>();

            CreateMap<Auction, AuctionUpdated>()
                .IncludeMembers(a => a.Item);

            CreateMap<Item, AuctionUpdated>();
        }
    }
}

using AuctionContracts;
using AutoMapper;
using BiddingService.Domain.DTOs;
using BiddingService.Domain.Models;

namespace BiddingService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bid, BidDto>();
            CreateMap<Bid, BidPlaced>();
        }
    }
}

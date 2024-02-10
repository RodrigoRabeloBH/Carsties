using AuctionContracts;
using AuctionService.Domain.DTOs;
using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces.Repository;
using AuctionService.Domain.Interfaces.Services;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuctionService.Application.Services
{
    public class AuctionServices : IAuctionServices
    {
        private readonly ILogger<AuctionServices> _logger;
        private readonly IAuctionRepository _rep;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuctionServices(ILogger<AuctionServices> logger, IAuctionRepository rep, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _rep = rep;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<AuctionDto> Create(CreateAuctionDto createAuctionDto, string seller)
        {
            _logger.LogInformation("[AUCTION SERVICES][CREATE] --> Creating auction ...");

            var auction = _mapper.Map<Auction>(createAuctionDto);

            auction.Seller = seller;

            await _rep.Create(auction);

            var newAuction = _mapper.Map<AuctionDto>(auction);

            var auctionCreated = _mapper.Map<AuctionCreated>(newAuction);

            await _publishEndpoint.Publish(auctionCreated);

            await _rep.Save();

            _logger.LogInformation("[AUCTION SERVICES][CREATE] --> Auction created and published to RabbitMq");

            return newAuction;
        }

        public async Task<bool> DeleteById(Guid id, string seller)
        {
            _logger.LogInformation("[AUCTION SERVICES][DELETE] --> Removing auction id: {id}", id);

            var auction = await _rep.GetAuctionById(id);

            if (auction.Seller != seller) return false;

            var auctionDeleted = new AuctionDeleted { Id = id.ToString() };

            await _publishEndpoint.Publish(auctionDeleted);

            return await _rep.DeleteById(auction);
        }

        public async Task<List<AuctionDto>> GetAll(string date)
        {
            _logger.LogInformation("[AUCTION SERVICES][GET ALL] --> Getting all auctions...");

            var auctions = await _rep.GetAllAuction(date);

            return _mapper.Map<List<AuctionDto>>(auctions);
        }

        public async Task<AuctionDto> GetById(Guid id)
        {
            _logger.LogInformation("[AUCTION SERVICES][GET BY ID] --> Getting auction by id: {id}", id);

            var auction = await _rep.GetAuctionById(id);

            return _mapper.Map<AuctionDto>(auction);
        }

        public async Task<AuctionDto> Update(Guid id, UpdateAuctionDto updateAuctionDto, string seller)
        {
            _logger.LogInformation("[AUCTION SERVICES][UPDATE] --> Updating auction started...");

            var auction = await _rep.GetAuctionById(id);

            if (auction is null) return null;

            if (auction.Seller != seller) return null;

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
            auction.Item.ImageUrl = updateAuctionDto.ImageUrl ?? auction.Item.ImageUrl;

            var auctionUpdated = _mapper.Map<AuctionUpdated>(auction);

            await _publishEndpoint.Publish(auctionUpdated);

            await _rep.Update(auction);

            return _mapper.Map<AuctionDto>(auction);
        }
    }
}

using AuctionService.Domain.DTOs;

namespace AuctionService.Domain.Interfaces.Services
{
    public interface IAuctionServices
    {
        Task<List<AuctionDto>> GetAll(string date);
        Task<AuctionDto> GetById(Guid id);
        Task<AuctionDto> Create(CreateAuctionDto createAuctionDto);
        Task<AuctionDto> Update(Guid id, UpdateAuctionDto updateAuctionDto);
        Task<bool> DeleteById(Guid id);
    }
}

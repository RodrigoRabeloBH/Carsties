using AuctionService.Domain.DTOs;

namespace AuctionService.Domain.Interfaces.Services
{
    public interface IAuctionServices
    {
        Task<List<AuctionDto>> GetAll(string date);
        Task<AuctionDto> GetById(Guid id);
        Task<AuctionDto> Create(CreateAuctionDto createAuctionDto, string seller);
        Task<AuctionDto> Update(Guid id, UpdateAuctionDto updateAuctionDto, string seller);
        Task<bool> DeleteById(Guid id, string seller);
    }
}

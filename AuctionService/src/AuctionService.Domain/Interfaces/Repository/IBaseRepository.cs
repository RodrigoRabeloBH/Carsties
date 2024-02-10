using AuctionService.Domain.Entities;

namespace AuctionService.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T> : IDisposable where T : Entity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> DeleteById(Guid id);
    }
}

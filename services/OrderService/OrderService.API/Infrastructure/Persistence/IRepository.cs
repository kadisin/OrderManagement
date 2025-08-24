using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence
{
    public interface IRepository
    {
        Task AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid id);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
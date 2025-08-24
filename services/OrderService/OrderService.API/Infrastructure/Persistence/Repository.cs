using Infrastructure.Persistence;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence
{
    public class Repository : IRepository
    {
        private readonly OrderDbContext _context;

        public Repository(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
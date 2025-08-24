using MediatR;
using OrderService.API.Infrastructure.Persistence;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateOrderCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(Guid.NewGuid(), request.CustomerName);
            await _repository.AddAsync(order);
            await _repository.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
    }
}
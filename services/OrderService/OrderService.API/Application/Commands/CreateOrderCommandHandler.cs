using MediatR;
using OrderService.API.Infrastructure.Persistence;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Messaging.Topics;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IRepository _repository;
        private readonly IMessagePublisher _messagePublisher;

        public CreateOrderCommandHandler(IRepository repository, IMessagePublisher messagePublisher)
        {
            _repository = repository;
            _messagePublisher = messagePublisher;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Add to db
            var order = new Order(Guid.NewGuid(), request.CustomerName);
            await _repository.AddAsync(order);
            await _repository.SaveChangesAsync(cancellationToken);
            
            // Send message
            var message = new OrderCreatedMessage
            {
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                CreatedAt = order.CreatedAt
            };

            await _messagePublisher.PublishAsync(message, "order-created-topic");

            return order.Id;
        }
    }
}
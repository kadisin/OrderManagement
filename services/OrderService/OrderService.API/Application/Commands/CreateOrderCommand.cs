using MediatR;

namespace OrderService.Application.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public string CustomerName { get; set; }

        public CreateOrderCommand(string customerName)
        {
            CustomerName = customerName;
        }
    }
}
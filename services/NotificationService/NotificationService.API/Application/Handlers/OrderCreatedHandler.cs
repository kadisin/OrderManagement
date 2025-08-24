using Microsoft.Extensions.Logging;

namespace NotificationService.API.Application.Handlers
{
    public class OrderCreatedHandler
    {
        private readonly ILogger<OrderCreatedHandler> _logger;

        public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(string message)
        {
            _logger.LogInformation("Received OrderCreated event with message: {Message}", message);

            // TODO: Implement email notification or other logic

            return Task.CompletedTask;
        }
    }
}
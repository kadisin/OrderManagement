using Azure.Messaging.ServiceBus;
using System.Text.Json;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Messaging
{
    public class AzureServiceBusPublisher : IMessagePublisher
    {
        private readonly ServiceBusClient _client;

        public AzureServiceBusPublisher(ServiceBusClient client)
        {
            _client = client;
        }

        public async Task PublishAsync<T>(T message, string topicName)
        {
            ServiceBusSender sender = _client.CreateSender(topicName);

            string body = JsonSerializer.Serialize(message);
            var serviceBusMessage = new ServiceBusMessage(body);

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
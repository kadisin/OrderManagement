using Azure.Messaging.ServiceBus;
using NotificationService.API.Application.Handlers;
using NotificationService.API.Domain.Events;
using System.Text.Json;

namespace NotificationService.API.AzureServiceBus;

public class ServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<ServiceBusConsumer> _logger;
    private readonly OrderCreatedHandler _orderCreatedHandler;


    public ServiceBusConsumer(IConfiguration config, ILogger<ServiceBusConsumer> logger, OrderCreatedHandler orderCreatedHandler)
    {
        _logger = logger;
        _orderCreatedHandler = orderCreatedHandler;

        var client = new ServiceBusClient(config["AzureServiceBus:ConnectionString"]);
        _processor = client.CreateProcessor(
            config["AzureServiceBus:TopicName"],
            config["AzureServiceBus:SubscriptionName"],
            new ServiceBusProcessorOptions()
        );
    }

    public async Task StartProcessingAsync()
    {
        _processor.ProcessMessageAsync += ProcessMessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync();
    }

    public async Task StopProcessingAsync()
    {
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
    }

    private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(body);

        await _orderCreatedHandler.HandleAsync(body);

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error in Service Bus Processor");
        return Task.CompletedTask;
    }
}
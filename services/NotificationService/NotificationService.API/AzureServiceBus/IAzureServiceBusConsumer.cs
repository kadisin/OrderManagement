namespace NotificationService.API.AzureServiceBus;

public interface IAzureServiceBusConsumer
{
    Task StartProcessingAsync();
    Task StopProcessingAsync();
}
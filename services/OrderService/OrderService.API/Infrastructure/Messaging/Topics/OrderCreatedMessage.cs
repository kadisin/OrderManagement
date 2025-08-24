namespace OrderService.Infrastructure.Messaging.Topics
{
    public class OrderCreatedMessage
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

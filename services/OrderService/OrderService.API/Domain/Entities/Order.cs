namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order(Guid id, string customerName)
        {
            Id = id;
            CustomerName = customerName;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
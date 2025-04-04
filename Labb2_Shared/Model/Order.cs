namespace Labb2_Shared.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

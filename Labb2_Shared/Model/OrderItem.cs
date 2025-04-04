namespace Labb2_Shared.Model
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }
}

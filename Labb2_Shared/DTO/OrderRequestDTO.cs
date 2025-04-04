namespace Labb2_Shared.DTO;

public class OrderRequestDTO
{
    public Guid CustomerId { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
}
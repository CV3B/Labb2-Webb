using Labb2_Shared.Model;

namespace Labb2_Shared.DTO;

public class CartItemDTO
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
}
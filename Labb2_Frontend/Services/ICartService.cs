using Labb2_Shared.DTO;
using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public interface ICartService
{
    List<CartItemDTO> GetItemsFromCart();
    void AddToCart(Product product);
    void RemoveFromCart(CartItemDTO cartItem);
    void EmptyCart();
}
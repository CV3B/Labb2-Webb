using Labb2_Shared.DTO;
using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public class CartService : ICartService
{
    private List<CartItemDTO> CartItems { get; set; } = new();


    public List<CartItemDTO> GetItemsFromCart()
    {
        return CartItems;
    }

    public void AddToCart(Product product)
    {
        var existingCartItem = CartItems.FirstOrDefault(ci => ci.Product.Id == product.Id);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity++;
        }
        else
        {
            CartItemDTO cartItem = new()
            {
                Product = product,
                Quantity = 1
            };

            CartItems.Add(cartItem);
        }
    }

    public void RemoveFromCart(CartItemDTO cartItem)
    {
        var existingCartItem = CartItems.FirstOrDefault(ci => ci.Product.Id == cartItem.Product.Id);
        if (existingCartItem != null)
        {
            CartItems.Remove(existingCartItem);
        }
    }

    public void EmptyCart()
    {
        CartItems.Clear();
    }
}
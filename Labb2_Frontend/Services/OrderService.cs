using System.Net.Http.Json;
using Labb2_Shared.DTO;
using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public class OrderService(HttpClient httpClient) : IOrderService
{
    public async Task<List<Order>> GetOrders()
    {
        return await httpClient.GetFromJsonAsync<List<Order>>("orders") ?? new List<Order>();
    }

    public async Task<List<Order>> GetOrderById(Guid id)
    {
        return await httpClient.GetFromJsonAsync<List<Order>>($"orders/search?customerId={id}") ?? new List<Order>();
    }

    public async Task<Order> GetOrderByOrderId(Guid id)
    {
        return await httpClient.GetFromJsonAsync<Order>($"orders/search?orderId={id}");
    }

    public async Task<bool> AddOrder(OrderRequestDTO order)
    {
        var response = await httpClient.PostAsJsonAsync("orders", order);
        return response.IsSuccessStatusCode;
    }
}
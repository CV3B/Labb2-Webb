using Labb2_Shared.DTO;
using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public interface IOrderService
{
    Task<List<Order>> GetOrders();
    Task<List<Order>> GetOrderById(Guid id);
    Task<Order> GetOrderByOrderId(Guid id);
    Task<bool> AddOrder(OrderRequestDTO order);
}
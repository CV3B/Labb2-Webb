
using Labb2_Shared.Model;

namespace Labb2_Api.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> GetOrdersByCustomerId(Guid id);
    Task<Order>  GetOrdersByOrderId(Guid id);
}
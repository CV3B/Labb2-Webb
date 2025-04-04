using Labb2_Api.Data;
using Labb2_Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Api.Repositories;

public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository
{
    private readonly AppDbContext context = context;

    public override async Task<IEnumerable<Order>> GetAll()
    {
        return await context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).Include(o => o.Customer)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByCustomerId(Guid id)
    {
        return await context.Orders
            .Where(o => o.CustomerId == id)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .ToListAsync();
    }

    public async Task<Order> GetOrdersByOrderId(Guid id)
    {
        return await context.Orders
            .Where(o => o.Id == id)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync() ?? new Order();
    }
}
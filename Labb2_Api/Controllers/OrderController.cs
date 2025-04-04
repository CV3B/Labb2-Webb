using Labb2_Api.Data;
using Labb2_Shared.DTO;
using Labb2_Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb2_Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController(IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    /// Gets all the orders in the database
    /// </summary>
    /// <returns>Returns a list of all the orders</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await unitOfWork.OrderRepository.GetAll();
    }

    /// <summary>
    /// Search via a customer id or an order id
    /// </summary>
    /// <param name="customerId">Id of a customer</param>
    /// <param name="orderId">Id of an order</param>
    /// <returns>Returns a list of orders if searched with customer id or returns one order if searched with order id</returns>
    [HttpGet("search")]
    public async Task<IActionResult> GetOrdersBySearch(Guid customerId, Guid orderId)
    {
        if (!customerId.Equals(Guid.Empty))
        {
            var orders = await unitOfWork.OrderRepository.GetOrdersByCustomerId(customerId);
            return orders is not null ? Ok(orders) : NotFound("Order not found");
        }

        if (!orderId.Equals(Guid.Empty))
        {
            var order = await unitOfWork.OrderRepository.GetOrdersByOrderId(orderId);
            return order is not null ? Ok(order) : NotFound("Order not found");
        }

        return BadRequest("Id is invalid");
    }

    /// <summary>
    /// Creates a new order
    /// </summary>
    /// <param name="orderDTO">Customer id and a list of order items</param>
    /// <returns>Returns 200 with the order or 404 Not Found if it failed</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] OrderRequestDTO orderDTO)
    {
        var customer = await unitOfWork.CustomerRepository.GetById(orderDTO.CustomerId);
        if (customer == null)
            return NotFound("Customer not found");

        var newOrder = new Order
        {
            CustomerId = orderDTO.CustomerId,
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in orderDTO.OrderItems)
        {
            var product = await unitOfWork.ProductRepository.GetById(item.ProductId);
            if (product == null)
                return NotFound("Product not found");

            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TotalPrice = product.Price * item.Quantity
            };

            newOrder.OrderItems.Add(orderItem);
        }

        await unitOfWork.OrderRepository.Add(newOrder);
        await unitOfWork.Save();

        return Ok(newOrder);
    }
}
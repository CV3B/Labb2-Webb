using Labb2_Api.Data;
using Labb2_Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb2_Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomerController(IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    /// Gets all the Customers from the database
    /// </summary>
    /// <returns>Returns a list of all the customers</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IEnumerable<Customer>> GetCustomers()
    {
        return await unitOfWork.CustomerRepository.GetAll();
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="customer">Customer data of the new customer</param>
    [Authorize]
    [HttpPost]
    public async void AddCustomer([FromBody] Customer customer)
    {
        await unitOfWork.CustomerRepository.Add(customer);
        await unitOfWork.Save();
    }

    /// <summary>
    /// Updates a customer with new data
    /// </summary>
    /// <param name="id">Id of the customer that is going to be updated</param>
    /// <param name="customer">Customer data that is going to be updated</param>
    /// <returns>Returns 200 when the customer is updated or 404 if the id does not exist as a custommr</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] Customer customer)
    {
        var customerToUpdate = await unitOfWork.CustomerRepository.GetById(id);
        if (customerToUpdate == null) return NotFound("Customer not found");

        customerToUpdate.FirstName = customer.FirstName;
        customerToUpdate.LastName = customer.LastName;
        customerToUpdate.Email = customer.Email;
        customerToUpdate.Phone = customer.Phone;
        customerToUpdate.Address = customer.Address;

        unitOfWork.CustomerRepository.Update(customerToUpdate);
        await unitOfWork.Save();

        return Ok(customerToUpdate);
    }

    /// <summary>
    /// Get a customer by email
    /// </summary>
    /// <param name="email">Email of the user</param>
    /// <returns>Returns the user</returns>
    [HttpGet("search")]
    public async Task<Customer> GetCustomerByEmail(string email)
    {
        return await unitOfWork.CustomerRepository.GetByEmail(email);
    }
}
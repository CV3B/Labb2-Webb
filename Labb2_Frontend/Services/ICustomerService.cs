using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetCustomers();
    Task<bool> AddCustomer(Customer customer);
    Task<bool> UpdateCustomer(Customer customer);
    Task<Customer?> GetCustomerByEmail(string email);
}
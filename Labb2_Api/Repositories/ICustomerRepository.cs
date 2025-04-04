
using Labb2_Shared.Model;

namespace Labb2_Api.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer> GetByEmail(string email);
}
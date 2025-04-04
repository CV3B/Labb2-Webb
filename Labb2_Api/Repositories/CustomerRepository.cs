using Labb2_Api.Data;
using Labb2_Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Api.Repositories;

public class CustomerRepository(AppDbContext context) : Repository<Customer>(context), ICustomerRepository
{
    private readonly AppDbContext context = context;

    public async Task<Customer> GetByEmail(string email)
    {
        return await context.Customers.FirstOrDefaultAsync(x => x.Email == email) ?? new Customer() {FirstName = "Customer does not exist"};
    }
}
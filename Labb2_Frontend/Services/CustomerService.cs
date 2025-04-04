using System.Net.Http.Json;
using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public class CustomerService(HttpClient httpClient) : ICustomerService
{
    public async Task<IEnumerable<Customer>> GetCustomers()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<Customer>>("customers") ?? new List<Customer>();
    }

    public async Task<bool> AddCustomer(Customer customer)
    {
        var response = await httpClient.PostAsJsonAsync("customers", customer);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCustomer(Customer customer)
    {
        var response = await httpClient.PutAsJsonAsync($"customers/{customer.Id}", customer);
        return response.IsSuccessStatusCode;
    }

    public async Task<Customer?> GetCustomerByEmail(string email)
    {
        return await httpClient.GetFromJsonAsync<Customer>($"customers/search?email={email}");
    }
}
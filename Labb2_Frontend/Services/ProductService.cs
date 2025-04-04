using System.Net.Http.Json;
using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public class ProductService(HttpClient httpClient) : IProductService
{
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<Product>>("products") ?? new List<Product>();
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await httpClient.GetFromJsonAsync<Product>($"products/search?id={id}");
    }

    public async Task<Product?> GetProductByName(string name)
    {
        return await httpClient.GetFromJsonAsync<Product>($"products/search?name={name}");
    }

    public async Task<bool> AddProduct(Product product)
    {
        var response = await httpClient.PostAsJsonAsync("products", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var response = await httpClient.PutAsJsonAsync($"products/{product.Id}", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        var response = await httpClient.DeleteAsync($"products/{id}");
        return response.IsSuccessStatusCode;
    }
}
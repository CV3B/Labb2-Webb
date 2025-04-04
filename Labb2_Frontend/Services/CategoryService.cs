using System.Net.Http.Json;
using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public class CategoryService(HttpClient httpClient) : ICategoryService
{
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<Category>>("categories") ?? new List<Category>();
    }

    public async Task<bool> AddCategory(Category category)
    {
        var response = await httpClient.PostAsJsonAsync("categories", category);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        var response = await httpClient.PutAsJsonAsync($"categories/{category.Id}", category);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        var response = await httpClient.DeleteAsync($"categories/{categoryId}");
        return response.IsSuccessStatusCode;
    }
}
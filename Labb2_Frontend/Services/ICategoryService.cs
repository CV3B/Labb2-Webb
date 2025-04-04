using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategories();
    Task<bool> AddCategory(Category category);
    Task<bool> UpdateCategory(Category category);
    Task<bool> DeleteCategory(Guid categoryId);
}
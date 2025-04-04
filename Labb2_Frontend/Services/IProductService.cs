using Labb2_Shared.Model;

namespace Labb2_Frontend.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProducts();
    Task<Product?> GetProductById(Guid id);
    Task<Product?> GetProductByName(string name);
    Task<bool> AddProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(Guid id);
}
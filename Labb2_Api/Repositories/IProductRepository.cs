using Labb2_Shared.Model;

namespace Labb2_Api.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task MarkAsDiscontinued(Guid id);

    Task<Product> GetByName(string name);
    

}
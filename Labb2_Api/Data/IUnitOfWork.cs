using Labb2_Api.Repositories;

namespace Labb2_Api.Data;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IOrderRepository OrderRepository { get; }
    ICategoriesRepository CategoriesRepository { get; }

    Task Save();
}
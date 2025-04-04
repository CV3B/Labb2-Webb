using Labb2_Api.Repositories;

namespace Labb2_Api.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext context;

    private IProductRepository _productRepository;

    public IProductRepository ProductRepository
    {
        get { return _productRepository ??= new ProductRepository(context); }
    }

    private ICustomerRepository _customerRepository;

    public ICustomerRepository CustomerRepository
    {
        get { return _customerRepository ??= new CustomerRepository(context); }
    }

    private IOrderRepository _orderRepository;

    public IOrderRepository OrderRepository
    {
        get { return _orderRepository ??= new OrderRepository(context); }
    }

    private ICategoriesRepository _categoriesRepository;

    public ICategoriesRepository CategoriesRepository
    {
        get { return _categoriesRepository ??= new CategoriesRepository(context); }
    }

    public UnitOfWork(AppDbContext context)
    {
        this.context = context;
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }
}
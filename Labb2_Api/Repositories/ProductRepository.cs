using Labb2_Api.Data;
using Labb2_Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Api.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    private readonly AppDbContext context = context;
    private readonly DbSet<Product> set = context.Set<Product>();

    public override async Task<IEnumerable<Product>> GetAll()
    {
        return await this.set.Include(x => x.Category).ToListAsync();
    }

    public override async Task<Product> GetById(Guid id)
    {
        return await this.set.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id) ?? new Product() {Name = "Something went wrong"};
    }

    public async Task MarkAsDiscontinued(Guid id)
    {
        var product = await GetById(id);
        if (product != null)
        {
            product.Status = Status.Discontinued;
            context.Products.Update(product);
        }
    }

    public async Task<Product> GetByName(string name)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Name == name);
        return product ?? new Product { Name = "Product does not exist" };
    }
}
using Labb2_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Api.Repositories;

public class Repository<TEntity>(AppDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    private readonly AppDbContext context = context;
    private readonly DbSet<TEntity> set = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await set.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await set.ToListAsync();
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        var entry = await set.AddAsync(entity);
        return entry.Entity;
    }

    public void Update(TEntity entity)
    {
        set.Update(entity);
    }

    public async Task Delete(Guid id)
    {
        var entity = await set.FindAsync(id);
        if (entity != null)
        {
            set.Remove(entity);
        }
    }
}
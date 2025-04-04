namespace Labb2_Api.Repositories;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> Add(TEntity entity);
    void Update(TEntity entity);
    Task Delete(Guid id);
}
namespace Common.Repository;

public interface IReadonlyRepository<TEntity, in TId>
    where TEntity : class
{
    IEnumerable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync();
    TEntity GetById(TId id);
    Task<TEntity> GetByIdAsync(TId id);
}
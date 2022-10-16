namespace Common.Repository;

public interface IRepository<TEntity, in TId> : IReadonlyRepository<TEntity, TId>
    where TEntity : class
{
    void Insert(TEntity obj);
    Task InsertAsync(TEntity obj);
    void Update(TEntity obj);
    Task UpdateAsync(TEntity obj);
    void Delete(TId id);
    Task DeleteAsync(TId id);
    void Save();
    Task SaveAsync();
}
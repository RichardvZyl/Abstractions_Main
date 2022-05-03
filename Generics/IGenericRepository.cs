using Abstractions.Domain;

namespace Abstractions.Generics;

public interface IGenericRepository<TEntity, TId> where TEntity : Entity<TId> where TId : struct
{
    Task<TId> CreateAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(TId id);
    Task<IEnumerable<TEntity>> ListAsync();
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TId id);

    Task<IDictionary<TId, TEntity>> GetTableAsync();
}


namespace Abstractions.Generics;

public interface IGenericRepository<T, TId>
{
    Task<TId> Create(T model);
    Task<T> GetById(TId id);
    Task<IEnumerable<T>> ReadList();
    Task Update(T model);
    Task Delete(TId id);

    Task<IDictionary<TId, T>> GetTable();
}

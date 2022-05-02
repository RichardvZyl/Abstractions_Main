namespace Abstractions.Generics;
public interface IGenericRepository<T, TId>
{
    Task<TId> Create(T model);
    Task<IEnumerable<T>> ReadList();
    Task Update(T model);
    Task Delete(TId id);
}

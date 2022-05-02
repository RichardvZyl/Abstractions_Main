using Abstractions.Results;

namespace Abstractions.Generics;

public interface IGenericService<T, TId>
{
    Task<IResult<TId>> Create(T model);
    Task<IResult<T>> Get(TId id);
    Task<IResult<IEnumerable<T>>> GetList();
    Task<IResult> Update(T model);
    Task<IResult> Delete(TId id);
}

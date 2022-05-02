using Abstractions.Results;

namespace Abstractions.Generics;

public interface IGenericService<T, TId>
{
    Task<IResult<TId>> Create(T model);
    Task<IResult<IEnumerable<T>>> ReadList();
    Task<IResult> Update(T model);
    Task<IResult> Delete(TId id);
}

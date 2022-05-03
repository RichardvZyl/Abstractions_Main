using Abstractions.Domain;
using Abstractions.Results;

namespace Abstractions.Generics;

public interface IGenericService<TEntity, UModel, TId> where TEntity : Entity<TId> where UModel : new() where TId : struct
{
    Task<IResult<TId>> CreateAsync(UModel model);
    Task<IResult<UModel>> GetAsync(TId id);
    Task<IResult<IEnumerable<UModel>>> ListAsync();
    Task<IResult> UpdateAsync(UModel model);
    Task<IResult> DeleteAsync(TId id);
}

using Abstractions.Results;
using FluentValidation;

namespace Abstractions.Validator;

public abstract class Validator<T> : AbstractValidator<T>
{
    public new async Task<IResult> ValidateAsync(T instance, CancellationToken cancellation = default)
    {
        if (instance == null) return await Result.FailAsync();

        var result = await base.ValidateAsync(instance, cancellation);

        return result.IsValid
            ? await Result.SuccessAsync()
            : await Result.FailAsync(result.ToString());
    }
}

using Abstractions.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using IResult = Abstractions.Results.IResult;

namespace Abstractions.Validator;


// Implement when .Net 7.0 releases
//public class ResultFilter<T> : IRouteHandlerFilter where T : class
//{
//    private readonly IValidator<T> _validator;

//    public ResultFilter(IValidator<T> validator) => _validator = validator;

//    // this is a temporary work around to allow for the Result pattern in minimal API's I only later realized I would be returning the result object which is not what I wanted
//    // but it also allows me to abstract validation!
//    public async ValueTask<object?> InvokeAsync(RouteHandlerInvocationContext context, RouteHandlerFilterDelegate next)
//    {
//        var validatable = context.Parameters.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;

//        if (validatable is null)
//            return await Results.Result.FailAsync(string.Empty, 400);

//        var validationResult = await _validator.ValidateAsync(validatable);

//        if (!validationResult.IsValid)
//            return await Result.FailAsync(validationResult.Errors.ToString(), 204);

//        // before endpoint call // used for validation or middleware
//        var result = await next(context);
//        // after endpoint call I can now execute my result
//        return Task.FromResult(result as IResult ?? await Results.Result.SuccessAsync(string.Empty, 204))?.ResultAsync();
//    }
//}

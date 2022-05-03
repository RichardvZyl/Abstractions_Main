using Microsoft.AspNetCore.Http;

namespace Abstractions.Results;

// Implement when .Net 7.0 releases
//public class ResultFilter : IRouteHandlerFilter
//{
//    public ResultFilter()
//    {
//        // constructor based dependency injection works here!
//    }

//    // this is a temporary work around to allow for the Result pattern in minimal API's I only later realized I would be returning the result object which is not what I wanted
//    public async ValueTask<object?> InvokeAsync(RouteHandlerInvocationContext context, RouteHandlerFilterDelegate next)
//    {
//        // before endpoint call // used for validation or middleware
//        var result = await next(context);
//        // after endpoint call I can now execute my result
//        return Task.FromResult(result as IResult ?? await Result.SuccessAsync("", 204))?.ResultAsync();
//    }
//}

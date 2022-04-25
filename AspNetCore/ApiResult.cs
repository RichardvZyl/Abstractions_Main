using Abstractions.Results;
using Microsoft.AspNetCore.Mvc;

namespace Abstractions.AspNetCore;

public class ApiResult : IActionResult
{
    private readonly IResult _result;

    private ApiResult(IResult result)
        => _result = result;

    private ApiResult(object? data)
        => _result = Result<object>.Success(data);

    public static IActionResult Create(IResult result)
        => new ApiResult(result);

    public static IActionResult Create(object? data)
        => new ApiResult(data);

    public async Task ExecuteResultAsync(ActionContext context)
    {
        object? value = null;

        if (_result.Failed)
        {
            value = _result.Message;
        }
        else if (_result.GetType().IsGenericType && _result.GetType().GetGenericTypeDefinition() == typeof(Result<>))
        {
            value = (_result as dynamic)?.Data;
        }

        var objectResult = new ObjectResult(value)
        {
            StatusCode = _result.HttpResponseCode
        };

        await objectResult.ExecuteResultAsync(context);
    }
}

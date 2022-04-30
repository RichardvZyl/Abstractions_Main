using Microsoft.AspNetCore.Mvc;

namespace Abstractions.Results;

public static class ResultExtensions
{
    public static async Task<IActionResult> ResultAsync<T>(this Task<IResult<T>> result)
        => ApiResult.Create(await result.ConfigureAwait(false));

    public static async Task<IActionResult> ResultAsync(this Task<IResult> result)
        => ApiResult.Create(await result.ConfigureAwait(false));

    public static async Task<IActionResult> ResultAsync<T>(this Task<T> result)
        => ApiResult.Create(await result.ConfigureAwait(false));
}

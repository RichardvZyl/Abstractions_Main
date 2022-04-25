using Abstractions.Objects;
using Microsoft.AspNetCore.Http;

namespace Abstractions.AspNetCore;

public static class HttpRequestExtensions
{
    public static IList<BinaryFile> Files(this HttpRequest request)
    {
        var files = new List<BinaryFile>();

        foreach (var file in request.Form.Files)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            files.Add(new BinaryFile(Guid.NewGuid(), file.Name, memoryStream.ToArray(), file.Length, file.ContentType));
        }

        return files;
    }
}

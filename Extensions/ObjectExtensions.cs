using System.Text;
using System.Text.Json;

namespace Abstractions.Extensions;

public static class ObjectExtensions
{
    public static byte[] Bytes(this object obj)
        => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
}

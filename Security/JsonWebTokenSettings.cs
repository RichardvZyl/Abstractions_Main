namespace Abstractions.Security;

public class JsonWebTokenSettings
{
    public JsonWebTokenSettings
    (
        string key,
        TimeSpan expires
    )
    {
        Key = key;
        Expires = expires;
    }

    public JsonWebTokenSettings
    (
        string key,
        TimeSpan expires,
        string audience,
        string issuer
    ) : this(key, expires)
    {
        Audience = audience;
        Issuer = issuer;
    }

    public string Audience { get; } = string.Empty;

    public TimeSpan Expires { get; } = TimeSpan.Zero;

    public string Issuer { get; } = string.Empty;

    public string Key { get; } = string.Empty;
}

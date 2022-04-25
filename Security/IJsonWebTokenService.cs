using System.Security.Claims;

namespace Abstractions.Security;

public interface IJsonWebTokenService
{
    Dictionary<string, object> Decode(string token);

    string Encode(IList<Claim> claims);
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Abstractions.Security;

public class JsonWebTokenService : IJsonWebTokenService
{
    public JsonWebTokenService(JsonWebTokenSettings jsonWebTokenSettings)
    {
        JsonWebTokenSettings = jsonWebTokenSettings;

        var securityKey = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(JsonWebTokenSettings.Key));

        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
    }

    private JsonWebTokenSettings JsonWebTokenSettings { get; }

    private SigningCredentials SigningCredentials { get; }

    public Dictionary<string, object> Decode(string token)
        => new JwtSecurityTokenHandler().ReadJwtToken(token).Payload;

    public string Encode(IList<Claim> claims)
    {
        claims ??= new List<Claim>();

        var jwtSecurityToken = new JwtSecurityToken
        (
            JsonWebTokenSettings.Issuer,
            JsonWebTokenSettings.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.Add(JsonWebTokenSettings.Expires),
            SigningCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}

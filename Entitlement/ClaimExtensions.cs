using System.Security.Claims;

namespace Abstractions.Entitlement;

public static class ClaimExtensions
{
    //A claim is a name value pair that represents what the subject is, not what the subject can do.
    public static void AddJti(this ICollection<Claim> claims)
        => claims.Add(new Claim("jti", Guid.NewGuid().ToString()));

    public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        => roles.ToList().ForEach(role => claims.Add(new Claim("role", role)));

    public static void AddSub(this ICollection<Claim> claims, string sub)
        => claims.Add(new Claim("sub", sub));

    public static bool HasClaim(this List<Claim> claims, Claim claim) 
        => claims.Find(x => x.Value == claim.Value) != null;
}

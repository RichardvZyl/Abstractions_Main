using System.Reflection;
using System.Security.Claims;

namespace Abstractions.Entitlement;

public static class SavedClaims<T> where T : new()
{
    public static List<Claim> ApplyClaims(List<Claim> claims, T model)
    {//Add and remove claims based on entitlement saved in DB

        foreach (var prop in model?.GetType()?.GetProperties() ?? Array.Empty<PropertyInfo>())
        {
            if (prop.GetValue(model) == null)
                continue; //If null the property can be ignored

            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            if (type != typeof(bool))
                continue; //if type of boolean is not a claim then it can be ignored

            //Determine claim from static claims for project
            var claim = (Claim)(typeof(T)
                .GetMethods()
                .FirstOrDefault(x => x.Name == $"get_{prop.Name}")?
                .Invoke(null, null)!);

            if ((bool)prop.GetValue(model)!)
            {//Add
                if (!claims.HasClaim(claim))
                    claims.Add(claim); //If the claim is not yet present add it as per exceptions
            }
            else
            {//Remove
                if (claims.HasClaim(claim)) //If the claim is present remove it as per exceptions
                    claims.RemoveAt(claims.FindIndex(x => x.Value == claim?.Value)); //.Remove(claim) does not work
            }
        }

        return claims;
    }
}

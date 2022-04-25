using System.Security.Claims;

namespace Abstractions.Entitlement;

public static class ClaimExceptions<T> where T : IEntitlementExceptions, new()
{
    public static List<Claim> ApplyExceptions(List<Claim> claims, T model)
    {//Add and remove claims based on saved exceptions

        var expired = false;
        if (model.ExpiresOn != null) //Check if the exceptions have an expiry date if so determine if they are still valid
            expired = DateTimeOffset.Compare((DateTimeOffset)model.ExpiresOn, DateTimeOffset.UtcNow) < 1;

        if (expired)
            return claims; //if the exceptions have expired do not apply them

        foreach (var prop in model.GetType().GetProperties())
        {
            if (prop.GetValue(model) == null)
                continue; //If null the property can be ignored

            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            if (type != typeof(bool?))
                continue; //if type is not boolean it is not a claim and can be ignored

            //Determine claim from static claims for project
            var claim = (Claim)(typeof(T)
                .GetMethods()
                .FirstOrDefault(x => x.Name == $"get_{prop.Name}")?
                .Invoke(null, null)!);

            if ((bool)prop.GetValue(model)!)
            {//Add
                if (!claims.HasClaim(claim!))
                    claims.Add(claim!); //If the claim is not yet present add it as per exceptions
            }
            else
            {//Remove
                if (claims.HasClaim(claim!)) //If the claim is present remove it as per exceptions
                    claims.RemoveAt(claims.FindIndex(x => x.Value == claim?.Value)); //.Remove(claim) does not work
            }
        }

        return claims;
    }
}

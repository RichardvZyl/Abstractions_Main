using System.Reflection;
using System.Security.Claims;

namespace Abstractions.Entitlement;

public static class ClaimsSeedByRole<T> where T : new()
{
    public static T EntitlementFromClaims(List<Claim> claims)
    {//Build the EntitlementExceptions from the default claims per role
        var entitlementExceptions = new T();

        foreach (var claim in claims)
        {//Set each claim received to true for the EntitlementModel

            foreach (var prop in entitlementExceptions?.GetType()?.GetProperties() ?? Array.Empty<PropertyInfo>())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type != typeof(bool))
                    continue; //if type is not boolean it is not a claim and can be ignored 

                //Determine the claim from the static claims for this project
                var claimException = (Claim)(typeof(T)
                    .GetMethods()
                    .FirstOrDefault(x => x.Name == $"get_{prop.Name}")?
                    .Invoke(null, null)!);

                if (claim.Value == claimException?.Value)
                    prop.SetValue(entitlementExceptions, true); //set the claim to true if found on the list
            }
        }

        foreach (var prop in entitlementExceptions?.GetType()?.GetProperties() ?? Array.Empty<PropertyInfo>())
        {//Set each claim not received to false for the EntitlementModel

            //Determine the claim from the static claims for this project
            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            if (type != typeof(bool))
                continue; //if type is not boolean it is not a claim and can be ignored

            if (prop.GetValue(entitlementExceptions) == null)
                prop.SetValue(entitlementExceptions, false); //set the claim to false as it was not found in the list
        }

        return entitlementExceptions;
    }
}



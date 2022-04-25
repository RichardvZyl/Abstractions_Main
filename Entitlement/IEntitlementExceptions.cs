namespace Abstractions.Entitlement;

public interface IEntitlementExceptions
{
    public DateTimeOffset? ExpiresOn { get; set; }

}

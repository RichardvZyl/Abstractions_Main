namespace Abstractions.Security;

public interface ICryptographyService
{
    Task<string> Decrypt(string value);

    Task<string> Decrypt(string value, string salt);

    Task<string> Encrypt(string value);

    Task<string> Encrypt(string value, string salt);
}

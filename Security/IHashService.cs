namespace Abstractions.Security;

public interface IHashService
{
    string Create(string value, string salt);
}

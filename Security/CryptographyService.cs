using System.Security.Cryptography;
using System.Text;

namespace Abstractions.Security;

public class CryptographyService : ICryptographyService
{
    // TOD: Fix Crypto - borken after changing to .net 6.0 and changing to AES as previous algoritm became obsolete and even trying previous obsolute algoritm also no longer work - clearly a breaking change investigate
    // .net 6.0 breaking change - for more information: https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/partial-byte-reads-in-streams
    public CryptographyService(string cyrptoKey)
    {
        Key = cyrptoKey;
        if (string.IsNullOrEmpty(Key))
            throw new ArgumentNullException(nameof(cyrptoKey));
        MaxRead = Key.Length / numberOfBits;
    }

    private static int MaxRead { get; set; }
    private string Key { get; }
    private static readonly int numberOfBits = 8;

    public async Task<string> Decrypt(string value, string salt)
    {
        using var algorithm = Algorithm(salt);

        return Encoding.Unicode.GetString(await Transform(Convert.FromBase64String(value), algorithm.CreateDecryptor(), CryptoStreamMode.Read));
    }

    public async Task<string> Decrypt(string value)
        => await Decrypt(value, string.Empty);

    public async Task<string> Encrypt(string value, string salt)
    {
        using var algorithm = Algorithm(salt);

        return Convert.ToBase64String(await Transform(Encoding.Unicode.GetBytes(value), algorithm.CreateEncryptor(), CryptoStreamMode.Write));
    }
    
    public Task<string> Encrypt(string value)
        => Encrypt(value, string.Empty);

    private static async Task<byte[]> Transform(byte[] bytes, ICryptoTransform cryptoTransform, CryptoStreamMode cryptoMode)
    {
        using (cryptoTransform)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, cryptoMode);

            switch (cryptoMode)
            {
                case CryptoStreamMode.Write:
                await cryptoStream.WriteAsync(bytes);
                break;

                case CryptoStreamMode.Read:
                {
                    var totalRead = 0;
                    while (totalRead < bytes.Length)
                    {
                        var countLeft = bytes.Length - totalRead;
                        var count = countLeft < MaxRead ? countLeft : MaxRead;
                        var bytesRead = await cryptoStream.ReadAsync(bytes.AsMemory(totalRead, countLeft));
                        totalRead += bytesRead;
                        if (bytesRead == 0)
                            break;
                    }

                    break;
                }

                default:
                throw new ArgumentOutOfRangeException(nameof(cryptoMode));
            }

            cryptoStream.Close();

            return memoryStream.ToArray();
        }
    }

    private SymmetricAlgorithm Algorithm(string salt)
    {
        if (string.IsNullOrWhiteSpace(salt))
            salt = Key;

        using var key = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(Key), Encoding.Unicode.GetBytes(salt), 1, HashAlgorithmName.SHA256); // TODO: Sha256 is insecure
        using var algorithm = Aes.Create();

        algorithm.Padding = PaddingMode.PKCS7;
        algorithm.Key = key.GetBytes(algorithm.KeySize / numberOfBits);
        algorithm.IV = key.GetBytes(algorithm.BlockSize / numberOfBits);

        return algorithm;
    }
}

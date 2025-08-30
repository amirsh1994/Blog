using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Blog.Core.Services.HashServices;

public interface IPasswordHasher
{
    string Hash(string password);

    Task<(bool verfied, bool needsUpgrade)> CheckAsync(string databaseHash, string password);
}

public class PasswordHasher(IOptions<HashOption> hashOptions) : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public HashOption Option { get; } = hashOptions.Value;


    public string Hash(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Option.Iteration, HashAlgorithmName.SHA512);
        var finalHash = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);
        return $"{Option.Iteration}.{salt}.{finalHash}";
    }

    public async Task<(bool verfied, bool needsUpgrade)> CheckAsync(string databaseHash, string password)
    {
        var parts = databaseHash.Split(".", 3);

        if (parts is not [var iterationStrFromDb, var saltStrFromDb, var keyStrFromDb])
        {
            return (false, false);
        }

        if (!int.TryParse(iterationStrFromDb, out var iterationCount))
        {
            return (false, false);
        }

        var salt = Convert.FromBase64String(saltStrFromDb);
        var key = Convert.FromBase64String(keyStrFromDb);
        bool needsUpgrade = iterationCount != Option.Iteration;

        var valueTuple = await Task.Run(() =>
           {
               using var algorithm = new Rfc2898DeriveBytes(password, salt, iterationCount, HashAlgorithmName.SHA512);
               var hashToCheck = algorithm.GetBytes(KeySize);
               bool verified = hashToCheck.SequenceEqual(key);
               return (verified, needsUpgrade);
           });
        return valueTuple;
    }
}
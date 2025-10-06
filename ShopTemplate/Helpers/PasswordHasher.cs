using System.Security.Cryptography;
using ShopTemplate.Models.Interfaces;

namespace ShopTemplate.Helpers;

public class PasswordHasher : IPasswordHasher
{
    public (string PasswordHash, string Salt) ComputePasswordHash(string password)
    {
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        var salt = Convert.ToBase64String(saltBytes);
        var hash = ComputePasswordHash(password, saltBytes);
        return (hash, salt);
    }

    public string ComputePasswordHash(string password, byte[] saltBytes)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 101, HashAlgorithmName.SHA256);
        var hashBytes = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hashBytes);
    }
}
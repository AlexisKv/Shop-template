namespace ShopTemplate.Models.Interfaces;

public interface IPasswordHasher
{
    (string PasswordHash, string Salt) ComputePasswordHash(string password);
    string ComputePasswordHash(string password, byte[] saltBytes);
}
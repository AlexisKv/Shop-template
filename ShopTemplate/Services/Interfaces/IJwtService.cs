using ShopTemplate.Abstractions.Enums;

namespace ShopTemplate.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(string userId, string userEmail, Role role);
}
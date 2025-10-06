using ShopTemplate.Abstractions.Enums;

namespace ShopTemplate.Abstractions.Dto;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public Role Role { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
}
using System.ComponentModel.DataAnnotations;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.Abstractions.Enums;

namespace ShopTemplate.DB.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public string Username { get; set; }
    
    [MaxLength(256)]
    public string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Role Role { get; set; } = Role.Buyer;
    
    [MaxLength(128)]
    public string Salt { get; set; }
    
    public UserDto ToDto()
    {
        return new UserDto
        {
            CreatedAt = CreatedAt,
            Role = Role,
            UserId = Id,
            Username = Username,
            Salt = Salt,
            PasswordHash = PasswordHash
        };
    }

    public User FromDto( UserDto dto)
    {
        return new User()
        {
            CreatedAt = dto.CreatedAt,
            Role = dto.Role,
            Id = dto.UserId,
            Username = dto.Username,
            PasswordHash = dto.PasswordHash,
            Salt = dto.Salt
        };
    }
}
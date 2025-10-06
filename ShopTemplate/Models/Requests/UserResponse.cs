using ShopTemplate.Abstractions.Dto;
using ShopTemplate.Abstractions.Enums;

namespace ShopTemplate.Models.Requests;

public class UserResponse
{
    public string Username { get; set; }
    public Role Role { get; set; }

    public UserResponse FromDto(UserDto userDto)
    {
        return new UserResponse()
        {
            Username = userDto.Username,
            Role = userDto.Role
        };
    }
}
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Response;

public class LoginResponse
{
    public string JwtToken { get; set; }

    public LoginResponse FromDto(LoginDto dto)
    {
        return new LoginResponse {JwtToken = dto.JwtToken};
    }
}
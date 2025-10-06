using FluentResults;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Models.Interfaces;
using ShopTemplate.ResponseTypes;
using ShopTemplate.Services.Interfaces;

namespace ShopTemplate.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<UserDto>> RegisterUser(string username, string password)
    {
        var existingUser = await _userRepository.GetByUsername(username);


        if (existingUser != null)
        {
            return Result.Fail<UserDto>(new Error("User with this username already exists")
                .WithMetadata("Type", FailureTypes.AlreadyExists));
        }

        var userPassword = _passwordHasher.ComputePasswordHash(password);

        var newUser = new UserDto()
        {
            Username = username,
            PasswordHash = userPassword.PasswordHash,
            Salt =  userPassword.Salt,
        };

        await _userRepository.Add(newUser);

        return Result.Ok(newUser);
    }

    public async Task<Result<LoginDto>> Login(string username, string password)
    {
        var user = await _userRepository.GetByUsername(username);

        if (user == null)
            return Result.Fail(new Error("User with this username does not exist")
                .WithMetadata("Type", FailureTypes.NotFound));

        var hashedInputPassword = _passwordHasher.ComputePasswordHash(password, Convert.FromBase64String(user.Salt));

        
        if (hashedInputPassword != user.PasswordHash)
            return Result.Fail(new Error("Incorrect password")
                .WithMetadata("Type", FailureTypes.InvalidPassword));
        
        return Result.Ok(new LoginDto
        {
            JwtToken = _jwtService.GenerateToken(user.UserId.ToString(), user.Username, user.Role),
        });
    }
}
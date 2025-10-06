using FluentResults;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<UserDto>> GetById(Guid id)
    {
        var user = await _userRepository.GetById(id);

        if (user == null)
            return Result.Fail<UserDto>(new Error("User with this id does not exist")
                .WithMetadata("Type", FailureTypes.NotFound));
        
        
        return Result.Ok(user);
    }
}
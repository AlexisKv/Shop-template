using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Extensions;
using ShopTemplate.Helpers;
using ShopTemplate.Models.Requests;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

[ApiController]
[Route("api/user/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetUser()
    {
        var userId = UserHelper.GetUserId(User);
        
        var result = await _userService.GetById(userId);

        return result.ToActionResult(r => new UserResponse().FromDto(r));
    }

    [Authorize]
    [HttpPut("update-profile")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<UserResponse>> UpdateUser()
    {
        throw new NotImplementedException();
    }
    
    [Authorize]
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<UserResponse>> ChangePassword()
    {
        throw new NotImplementedException();
    }

}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Extensions;
using ShopTemplate.Models.Requests;
using ShopTemplate.Models.Response;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] UserRequest userRequest)
    {
        var result = await _authService.RegisterUser(userRequest.Username, userRequest.Password);
        
        //Basic user info is in the JWT token.
        return result.ToActionResult(r => new UserResponse().FromDto(r));
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] UserRequest userRequest)
    {
        var result = await _authService.Login(userRequest.Username, userRequest.Password);
        
        //Basic user info is in the JWT token.
        return result.ToActionResult(r => new LoginResponse().FromDto(r));
    }
    
    [AllowAnonymous]
    [HttpDelete("logout")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}
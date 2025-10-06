using Moq;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.Abstractions.Enums;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Models.Interfaces;
using ShopTemplate.Services;
using ShopTemplate.Services.Interfaces;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();

    private AuthService CreateService() =>
        new AuthService(_userRepoMock.Object, _passwordHasherMock.Object, _jwtServiceMock.Object);

    [Fact]
    public async Task RegisterUser_ShouldReturnUser_WhenUserDoesNotExist()
    {
        var username = "testuser";
        var password = "password";

        _userRepoMock.Setup(r => r.GetByUsername(username)).ReturnsAsync((UserDto?)null);
        _passwordHasherMock.Setup(h => h.ComputePasswordHash(password))
            .Returns(("hashedpass", "salt"));

        _userRepoMock.Setup(r => r.Add(It.IsAny<UserDto>())).Returns(Task.CompletedTask);

        var service = CreateService();

        var result = await service.RegisterUser(username, password);

        Assert.True(result.IsSuccess);
        Assert.Equal(username, result.Value.Username);
        Assert.Equal("hashedpass", result.Value.PasswordHash);
        Assert.Equal("salt", result.Value.Salt);
    }

    [Fact]
    public async Task RegisterUser_ShouldFail_WhenUserExists()
    {
        var username = "existing";
        _userRepoMock.Setup(r => r.GetByUsername(username))
            .ReturnsAsync(new UserDto { Username = username });

        var service = CreateService();

        var result = await service.RegisterUser(username, "password");

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("already exists"));
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        var username = "user";
        var password = "pass";
        var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var user = new UserDto
        {
            UserId = Guid.NewGuid(),
            Username = username,
            PasswordHash = "hashedpass",
            Salt = salt,
            Role = Role.Buyer
        };

        _userRepoMock.Setup(r => r.GetByUsername(username)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.ComputePasswordHash(password, Convert.FromBase64String(salt)))
            .Returns("hashedpass");

        _jwtServiceMock.Setup(j => j.GenerateToken(user.UserId.ToString(), username, user.Role))
            .Returns("token123");

        var service = CreateService();

        var result = await service.Login(username, password);

        Assert.True(result.IsSuccess);
        Assert.Equal("token123", result.Value.JwtToken);
    }

    [Fact]
    public async Task Login_ShouldFail_WhenPasswordIncorrect()
    {
        var username = "user";
        var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var user = new UserDto
        {
            UserId = Guid.NewGuid(),
            Username = username,
            PasswordHash = "hashedpass",
            Salt = salt
        };

        _userRepoMock.Setup(r => r.GetByUsername(username)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.ComputePasswordHash("wrongpass", Convert.FromBase64String(salt)))
            .Returns("wronghash");

        var service = CreateService();

        var result = await service.Login(username, "wrongpass");

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Incorrect password"));
    }

    [Fact]
    public async Task Login_ShouldFail_WhenUserDoesNotExist()
    {
        _userRepoMock.Setup(r => r.GetByUsername("nouser")).ReturnsAsync((UserDto?)null);

        var service = CreateService();

        var result = await service.Login("nouser", "pass");

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("does not exist"));
    }
}

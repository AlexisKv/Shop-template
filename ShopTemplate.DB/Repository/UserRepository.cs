using Microsoft.EntityFrameworkCore;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;

namespace ShopTemplate.DB.Repository;

public class UserRepository : IUserRepository
{
    private readonly ShopContext _dbContext;

    public UserRepository(ShopContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<UserDto?> GetById(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user?.ToDto();
    }

    public async Task<UserDto?> GetByUsername(string username)
    {
         var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());;
         return user?.ToDto();
    }

    public Task<IEnumerable<UserDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task Add(UserDto dto)
    {
        var user = new User().FromDto(dto);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public Task Update(UserDto user)
    {
        throw new NotImplementedException();
    }

    public Task Delete(UserDto user)
    {
        throw new NotImplementedException();
    }
}
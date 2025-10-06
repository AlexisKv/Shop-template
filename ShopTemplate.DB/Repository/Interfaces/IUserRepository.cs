using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.DB.Repository.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetById(Guid id);
    Task<UserDto?> GetByUsername(string username);
    Task<IEnumerable<UserDto>> GetAll();
    Task Add(UserDto user);
    Task Update(UserDto user);
    Task Delete(UserDto user);
}
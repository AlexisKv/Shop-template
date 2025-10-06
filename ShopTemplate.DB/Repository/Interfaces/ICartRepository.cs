using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.DB.Repository.Interfaces;

public interface ICartRepository
{
    Task<CartDto?> GetByUserId(Guid userId);
    Task AddNewCartWithItem(CartDto cart);
    Task AddItem(CartItemDto cart,int cartId, double total);
    Task RemoveItem(int cartId, int productId, double total);
    Task UpdateItemQuantity(int cartId, int productId, int quantity, double total);
}
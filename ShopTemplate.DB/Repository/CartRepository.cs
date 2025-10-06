using Microsoft.EntityFrameworkCore;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;

namespace ShopTemplate.DB.Repository;

public class CartRepository : ICartRepository
{
    private readonly ShopContext _shopContext;

    public CartRepository(ShopContext shopContext)
    {
        _shopContext = shopContext;
    }

    public async Task<CartDto?> GetByUserId(Guid userId)
    {
        var cart = await _shopContext.Cart
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        return cart?.ToDto();
    }

    public async Task AddNewCartWithItem(CartDto dto)
    {
        var cart = new Cart().FromDto(dto);
        await _shopContext.Cart.AddAsync(cart);
        await _shopContext.SaveChangesAsync();
    }
    
    public async Task RemoveItem(int cartId, int productId, double total)
    {
        var cart = await _shopContext.Cart
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        var item = cart!.Items.FirstOrDefault(i => i.ProductId == productId);
        
        if (item != null)
        {
            cart.Items.Remove(item);
            _shopContext.CartItem.Remove(item);
            cart.Total = total;
            await _shopContext.SaveChangesAsync();
        }
    }
    
    public async Task UpdateItemQuantity(int cartId, int productId, int quantity, double total)
    {
        var cart = await _shopContext.Cart
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.Quantity = quantity;
            cart.Total = total;
            await _shopContext.SaveChangesAsync();
        }
    }

    public async Task AddItem(CartItemDto dto, int cartID, double total)
    {
        var cart = await _shopContext.Cart
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartID);
        
        cart.Total = total;

        cart.Items.Add(new CartItem
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        });

        await _shopContext.SaveChangesAsync();
    }
}
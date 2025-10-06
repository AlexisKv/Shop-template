using System.ComponentModel.DataAnnotations;
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.DB.Models;

public class CartItem
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public Product? Product { get; set; }
    
    public CartItemDto ToDto()
    {
        return new CartItemDto
        {
            Id = Id,
            ProductId = ProductId,
            Quantity = Quantity,
            Product = Product?.ToDto()
        };
    }

    public CartItem FromDto(CartItemDto dto)
    {
        return new CartItem()
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Product = dto.Product != null ? new Product().FromDto(dto.Product) : null
        };
    }
}
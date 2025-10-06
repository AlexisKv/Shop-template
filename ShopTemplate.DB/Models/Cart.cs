using System.ComponentModel.DataAnnotations;
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.DB.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(36)]
    public Guid UserId { get; set; }
    
    public double Total { get; set; }
    
    public List<CartItem> Items { get; set; } = [];
    
    public CartDto ToDto()
    {
        return new CartDto
        {
            Id = Id,
            Items = Items.Select(x => x.ToDto()).ToList(),
            Total = Total,
            UserId = UserId
        };
    }

    public Cart FromDto(CartDto dto)
    {
        return new Cart
        {
            Id = dto.Id,
            Items = dto.Items.Select(x => new CartItem().FromDto(x)).ToList(),
            Total = dto.Total,
            UserId = dto.UserId
        };
    }
}
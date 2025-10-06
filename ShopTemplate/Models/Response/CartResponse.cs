using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Response;

public class CartResponse
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public double Total { get; set; }
    public List<CartItemResponse> Items { get; set; } = [];

    public CartResponse FromDto(CartDto cartDto)
    {
        return new CartResponse()
        {
            Id = cartDto.Id,
            UserId = cartDto.UserId,
            Total = cartDto.Total,
            Items = cartDto.Items.Select(x => new CartItemResponse().FromDto(x)).ToList()
        };
    }
}
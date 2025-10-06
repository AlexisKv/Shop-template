using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Hub;

public class CartHubResponse
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public double Total { get; set; }
    public List<CartItemHubResponse> Items { get; set; } = [];
    
    public CartHubResponse FromDto(CartDto dto)
    {
        return new CartHubResponse()
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Total = dto.Total,
            Items = dto.Items.Select(i => new CartItemHubResponse().FromDto(i)).ToList()
        };
    }
}
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Hub;

public class CartItemHubResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public ProductHubResponse? Product { get; set; }
    
    public CartItemHubResponse FromDto(CartItemDto dto)
    {
        return new CartItemHubResponse
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Product = dto.Product != null ? new ProductHubResponse().FromDto(dto.Product) : null
        };
    }
}
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Response;

public class CartItemResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public ProductResponse? Product { get; set; }

    public CartItemResponse FromDto(CartItemDto dto)
    {
        return new CartItemResponse()
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Product = dto.Product != null ? new ProductResponse().FromDto(dto.Product) : null
        };
    }
}
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Response;

public class ProductResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public double Price { get; set; }
    
    public string ImageLink { get; set; }

    public ProductResponse FromDto(ProductDto dto)
    {
        return new ProductResponse()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageLink = dto.ImageLink
        };
    }
}
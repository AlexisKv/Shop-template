using System.ComponentModel.DataAnnotations;
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.DB.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(200)]
    public string Name { get; set; }
    
    [MaxLength(2000)]
    public string Description { get; set; }
    
    public double Price { get; set; }
    
    public string ImageLink { get; set; }
    
    public ProductDto ToDto()
    {
        return new ProductDto
        {
            Id = Id,
            Name = Name,
            Price = Price,
            Description = Description,
            ImageLink = ImageLink
        };
    }

    public Product FromDto(ProductDto dto)
    {
        return new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageLink = dto.ImageLink
        };
    }
}
using System.ComponentModel.DataAnnotations;
using ShopTemplate.Abstractions.Dto;

namespace ShopTemplate.Models.Requests;

public class NewProductRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Range(0.01, double.MaxValue)]
    public double Price { get; set; }
    
    public IFormFile? ImageData { get; set; }
    
    public ProductDto ToDto()
    {
        return new ProductDto
        {
            Name = Name,
            Description = Description,
            Price = Price
        };
    }
}
using FluentResults;
using Microsoft.EntityFrameworkCore;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> AddProduct(ProductDto productDto, IFormFile? imageData )
    {
        var alreadyExists = await _productRepository.GetByName(productDto.Name) != null;

        if (alreadyExists)
        {
            return Result.Fail(new Error("Product with this name already exists")
                .WithMetadata("Type", FailureTypes.AlreadyExists));
        }
        
        string productImageLink;
        
        if (imageData!= null && imageData.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageData.FileName)}";
            var filePath = Path.Combine("Images", "Products", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageData.CopyToAsync(stream);
            }
            
            productImageLink = $"/Images/Products/{fileName}";
        }
        else
        {
            productImageLink = "/Images/Products/placeholder.png";
        }

        productDto.ImageLink = productImageLink;
        

        await _productRepository.Add(productDto);
        
        return Result.Ok();
    }
    
    public async Task<Result<PagedResultDto<ProductDto>>> GetAllProducts(int pageNumber = 1, int pageSize = 10)
    {
        var pagedResult = await _productRepository.GetPagedProductsAsync(pageNumber, pageSize);
        return Result.Ok(pagedResult);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Extensions;
using ShopTemplate.Models.Requests;
using ShopTemplate.Models.Response;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddProduct([FromForm] NewProductRequest newProductRequest)
    {
        var result = await _productService.AddProduct(newProductRequest.ToDto(), newProductRequest.ImageData);
        
        return result.ToActionResult();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResultResponse<ProductResponse>>> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _productService.GetAllProducts(pageNumber, pageSize);
        
        return result.ToActionResult(r => PagedResultResponse<ProductResponse>
            .FromDto(r, x => new ProductResponse().FromDto(x)));
    }
}
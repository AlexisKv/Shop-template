using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Extensions;
using ShopTemplate.Helpers;
using ShopTemplate.Models.Requests;
using ShopTemplate.Models.Response;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

[ApiController]
[Route("api/users/[controller]")]
public class CartController: ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddItemsToCart([FromBody] NewCartItemRequest cartRequest)
    {
        var userId = UserHelper.GetUserId(User);
        var result = await _cartService.AddItemToCart(userId, cartRequest.ProductId, cartRequest.Quantity);

        return result.ToActionResult();
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> RemoveItemFromCart([FromBody] int productId)
    {
        var userId = UserHelper.GetUserId(User);

        var result = await _cartService.RemoveItemFromCart(userId, productId);

        return result.ToActionResult();
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateItemQuantity([FromBody] UpdateItemQuantityRequest cartRequest)
    {
        var userId = UserHelper.GetUserId(User);

        var result = await _cartService.UpdateItemQuantity(userId, cartRequest.ProductId, cartRequest.Quantity);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetCartByUserId()
    {
        var userId = UserHelper.GetUserId(User);

        var result = await _cartService.GetCartByUserId(userId);
        return result.ToActionResult(r => new CartResponse().FromDto(r));
    }
}
using FluentResults;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly CartHubService _cartHubService;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository,
        CartHubService cartHubService)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _cartHubService = cartHubService;
    }

    public async Task<Result<CartDto>> GetCartByUserId(Guid userId)
    {
        var cart = await _cartRepository.GetByUserId(userId);
        if (cart == null)
        {
            return Result.Fail<CartDto>(new Error("Cart not found")
                .WithMetadata("Type", FailureTypes.NotFound));
        }

        await _cartHubService.Notify(userId, cart);
        return Result.Ok(cart);
    }

    public async Task<Result> AddItemToCart(Guid userId, int productId, int quantity)
    {
        var cart = await _cartRepository.GetByUserId(userId);
        var isNewCart = cart == null;

        if (isNewCart)
        {
            cart = new CartDto
            {
                UserId = userId,
                Items = new List<CartItemDto>()
            };
        }

        if (!isNewCart)
        {
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                return Result.Fail(new Error("Item already exists! Use update to change quantity")
                    .WithMetadata("Type", FailureTypes.AlreadyExists));
            }
        }

        var product = await _productRepository.GetById(productId);
        if (product == null)
            return Result.Fail(new Error("Product not found")
                .WithMetadata("Type", FailureTypes.NotFound));

        var newItem = new CartItemDto
        {
            ProductId = productId,
            Product = product,
            Quantity = quantity
        };


        UpdateCartTotal(cart);

        if (isNewCart)
            await _cartRepository.AddNewCartWithItem(cart);
        else
            await _cartRepository.AddItem(newItem, cart.Id, cart.Total);

        await _cartHubService.Notify(userId, cart);
        return Result.Ok();
    }

    public async Task<Result> RemoveItemFromCart(Guid userId, int productId)
    {
        var cart = await _cartRepository.GetByUserId(userId);
        if (cart == null) return null;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            cart.Items.Remove(item);
            UpdateCartTotal(cart);
            await _cartRepository.RemoveItem(cart.Id, productId, cart.Total);
        }

        await _cartHubService.Notify(userId, cart);
        return Result.Ok();
    }

    public async Task<Result> UpdateItemQuantity(Guid userId, int productId, int quantity)
    {
        var cart = await _cartRepository.GetByUserId(userId);
        if (cart == null)
            return Result.Fail(new Error("Cart not found")
                .WithMetadata("Type", FailureTypes.NotFound));

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.Quantity = quantity;
            UpdateCartTotal(cart);
            await _cartRepository.UpdateItemQuantity(cart.Id, productId, quantity, cart.Total);


            await _cartHubService.Notify(userId, cart);
            return Result.Ok();
        }


        return Result.Fail(new Error("Product not found in cart")
            .WithMetadata("Type", FailureTypes.NotFound));
    }

    private void UpdateCartTotal(CartDto cart)
    {
        cart.Total = cart.Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);
    }
}
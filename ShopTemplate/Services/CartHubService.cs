using Microsoft.AspNetCore.SignalR;
using ShopTemplate.Abstractions.Dto;
using ShopTemplate.Hubs;
using ShopTemplate.Models.Hub;

namespace ShopTemplate.Services;

public class CartHubService
{
    private readonly IHubContext<CartHub> _hubContext;

    public CartHubService(IHubContext<CartHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task Notify(Guid userId, CartDto cartDto)
    {
        var cartHubResponse = new CartHubResponse().FromDto(cartDto);
        await _hubContext.Clients.Group(userId.ToString()).SendAsync("CartUpdated", cartHubResponse);
    }
}
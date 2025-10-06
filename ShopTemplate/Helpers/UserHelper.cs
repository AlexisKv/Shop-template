using System.Security.Claims;

namespace ShopTemplate.Helpers;

public static class UserHelper
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        foreach (var claim in user.Claims)
        {
            Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
        }
        
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("UserId not found in token");

        return Guid.Parse(userIdClaim);
    }
}
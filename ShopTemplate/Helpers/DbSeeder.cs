using ShopTemplate.Abstractions.Enums;
using ShopTemplate.DB;
using ShopTemplate.DB.Models;

namespace ShopTemplate.Helpers;

public class DbSeeder
{
    public static async Task Seed(ShopContext context)
    {
        var hasher = new PasswordHasher();
        const string password = "TestPassword";
        var hashedPassword = hasher.ComputePasswordHash(password);
        var testUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var user = new User
        {
            Id = testUserId,
            Username = "TestUsername",
            PasswordHash = hashedPassword.PasswordHash,
            Salt = hashedPassword.Salt,
            Role = Role.Buyer
        };
        
        context.Users.Add(user);

        var products = new List<Product>
        {
            new Product
            {
                Id = 1, 
                Name = "Product 1",
                Description = "Sample 1", 
                Price = 12,
                ImageLink = "/Images/Products/placeholder.png"
            },
            new Product
            {
                Id = 2, 
                Name = "Product 2", 
                Description = "Sample 2", 
                Price = 15,
                ImageLink = "/Images/Products/placeholder.png"
            },
            new Product
            {
                Id = 3,
                Name = "Product 3",
                Description = "Sample product 3",
                Price = 20.0,
                ImageLink = "/Images/Products/placeholder.png"
            }
        };
        context.Products.AddRange(products);
        
        var cartItem = new CartItem
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };
        
        context.CartItem.Add(cartItem);
        
        var cart =new Cart
        {
            Id = 1,
            UserId = testUserId,
            Total = 12,
            Items = [cartItem]
        };

        context.Cart.Add(cart);



        await context.SaveChangesAsync();
    }
}
namespace ShopTemplate.Models.Requests;

public class NewCartItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
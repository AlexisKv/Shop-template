namespace ShopTemplate.Models.Requests;

public class UpdateItemQuantityRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
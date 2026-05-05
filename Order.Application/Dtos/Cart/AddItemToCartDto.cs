namespace Order.Application.Dtos.Cart;

public class AddItemToCartDto
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
}
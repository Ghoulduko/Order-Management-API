using Order.Application.Dtos.Product;

namespace Order.Application.Dtos.Cart;

public class CartItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int CartId { get; set; }
    public int ProductId{ get; set; }
    public ProductDto? Product { get; set; }
}
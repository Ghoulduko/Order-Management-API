using Order.Application.Dtos.Product;

namespace Order.Application.Dtos.Order;

public class OrderItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    
    public ProductDto Product { get; set; }
}
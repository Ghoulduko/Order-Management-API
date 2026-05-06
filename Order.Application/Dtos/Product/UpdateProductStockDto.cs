namespace Order.Application.Dtos.Product;

public class UpdateProductStockDto
{
    public int productId { get; set; }
    public int quantity { get; set; }
}
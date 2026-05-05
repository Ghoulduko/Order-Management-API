using Order.Application.Dtos.Payment;
using Order.Application.Dtos.User;

namespace Order.Application.Dtos.Order;

public class CustomerOrderDto
{
    public int Id { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public decimal Total { get; set; }
    
    public PaymentDto Payment { get; set; }
    
    public UserDto User { get; set; }
    public DateTime CreatedAt { get; set; }
}
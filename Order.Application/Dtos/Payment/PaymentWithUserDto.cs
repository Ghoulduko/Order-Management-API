using Order.Application.Dtos.User;

namespace Order.Application.Dtos.Payment;

public class PaymentWithUserDto
{
    public int Id { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public UserDto User { get; set; }
}
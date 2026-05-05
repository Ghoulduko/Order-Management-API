using Order.Core.Enums;

namespace Order.Application.Dtos.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}
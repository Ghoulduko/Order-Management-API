using Order.Core.Enums;

namespace Order.Application.Dtos.Payment;

public class AddPaymentDto
{
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public required int UserId { get; set; }
    public DateTime PaidAt { get; set; }
}
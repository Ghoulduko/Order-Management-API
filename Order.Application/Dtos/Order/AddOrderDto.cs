using Order.Application.Dtos.Payment;
using Order.Core.Enums;

namespace Order.Application.Dtos.Order;

public class AddOrderDto
{
    public PaymentMethod PaymentMethod { get; set; }
}
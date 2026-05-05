using Order.Core.Interfaces;

namespace Order.Core.Entities.PaymentTypes;

public class Paypal : IPaymentType
{
    public string ProcessPayment()
    {
        return "Paid with Paypal";
    }
}
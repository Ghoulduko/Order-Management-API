using Order.Core.Interfaces;

namespace Order.Core.Entities.PaymentTypes;

public class Crypto : IPaymentType
{
    public string ProcessPayment()
    {
        return "Paid with Crypto";
    }
}
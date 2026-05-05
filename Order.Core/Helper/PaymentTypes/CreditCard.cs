using Order.Core.Interfaces;

namespace Order.Core.Entities.PaymentTypes;

public class CreditCard : IPaymentType
{
    public string ProcessPayment()
    {
        return "Paid with a Creditcard";
    }
}
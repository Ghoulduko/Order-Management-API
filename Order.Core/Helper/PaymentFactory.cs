using Order.Core.Entities.PaymentTypes;
using Order.Core.Enums;
using Order.Core.Interfaces;

namespace Order.Core.Helper;

public class PaymentFactory : IPaymentFactory
{
    public IPaymentType ChoosePaymentMethod(PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.CreditCard => new CreditCard(),
            PaymentMethod.Crypto => new Crypto(),
            PaymentMethod.Paypal => new Paypal(),
            _ => throw new NotImplementedException($"Payment method {paymentMethod} is not implemented.")
        };
    }
}
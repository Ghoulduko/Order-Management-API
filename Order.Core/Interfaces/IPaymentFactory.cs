using Order.Core.Enums;

namespace Order.Core.Interfaces;

public interface IPaymentFactory
{
    public IPaymentType ChoosePaymentMethod(PaymentMethod paymentMethod);
}
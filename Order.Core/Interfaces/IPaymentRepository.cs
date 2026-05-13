using Order.Core.Entities;

namespace Order.Core.Interfaces;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<List<Payment>> GetAllUserPayments(int userId);
    Task<List<Payment>> GetAllUserPaymentsAdmin();
}
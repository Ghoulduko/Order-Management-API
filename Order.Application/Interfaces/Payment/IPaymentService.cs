using Order.Application.Dtos.Payment;
using Order.Core.Entities;

namespace Order.Application.Interfaces;

public interface IPaymentService
{
    Task Add(Payment request);
    Task<PaymentDto> GetById(int id);
    Task<List<PaymentDto>> GetAllUserPayments(int userId);
    Task<List<PaymentDto>> GetAll();
}
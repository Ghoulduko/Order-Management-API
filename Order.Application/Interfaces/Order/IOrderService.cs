using Order.Application.Dtos.Order;
using Order.Core.Enums;

namespace Order.Application.Interfaces;

public interface IOrderService
{
    Task Add(PaymentMethod paymentMethod, int userId, string userEmail);
    Task<List<CustomerOrderDto>> GetAll();
    Task<List<CustomerOrderDto>> GetAllUserOrders(int userId);
    Task<CustomerOrderDto> GetByIdForUser(int id, int userId);
    Task<CustomerOrderDto> GetByIdForAdmin(int id);
    Task CancelOrderById(int orderId, int userId);
}
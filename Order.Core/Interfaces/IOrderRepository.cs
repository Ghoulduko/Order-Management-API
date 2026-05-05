using Order.Core.Entities;

namespace Order.Core.Interfaces;

public interface IOrderRepository : IGenericRepository<CustomerOrder>
{
    Task<List<CustomerOrder>> GetAllUserOrders(int userId);
    Task<List<CustomerOrder>> GetAllOrders();
    Task<CustomerOrder> GetOrderById(int orderId);
}
using Microsoft.EntityFrameworkCore;
using Order.Core.Database;
using Order.Core.Entities;
using Order.Core.Enums;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Core.Helper;

public class OrderRepository : GenericRepository<CustomerOrder>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context) {}

    private IQueryable<CustomerOrder> BaseOrderQuery()
    {
        return _context.Orders
            .Include(o => o.Payment)
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
            .Include(i => i.User);
    }

    public async Task<List<CustomerOrder>> GetAllUserOrders(int userId)
    {
        return await BaseOrderQuery().Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task<List<CustomerOrder>> GetAllOrders()
    {
        return await BaseOrderQuery().ToListAsync();
    }

    public async Task<CustomerOrder> GetOrderById(int orderId)
    {
        var order = await BaseOrderQuery().FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new NotFoundException($"Order with id: {orderId} was not found");
        return order;
    }
}
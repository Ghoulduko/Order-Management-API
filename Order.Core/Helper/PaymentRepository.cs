using Microsoft.EntityFrameworkCore;
using Order.Core.Database;
using Order.Core.Entities;
using Order.Core.Interfaces;

namespace Order.Core.Helper;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(OrderDbContext context) : base(context){}

    public async Task<List<Payment>> GetAllUserPayments(int userId)
    {
        return await _context.Payments
            .Include(p => p.CustomerOrder)
            .Include(p => p.User)
            .Where(p => p.UserId == userId).ToListAsync();
    }
}
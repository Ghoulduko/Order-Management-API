using Microsoft.EntityFrameworkCore;
using Order.Core.Database;
using Order.Core.Entities;
using Order.Core.Enums;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Core.Helper;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    public CartRepository(OrderDbContext context) : base(context) {}
    
    private IQueryable<Cart> BaseCartQuery()
    {
        return _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product);
    }
    
    public async Task<List<Cart>> GetCartsForAdmin()
    {
        return await BaseCartQuery().Include(i => i.User).ToListAsync();
    }

    public async Task<Cart> GetCartWithItemsById(int id)
    {
        var cart = await BaseCartQuery().FirstOrDefaultAsync(c => c.Id == id);
        if (cart == null)
            throw new NotFoundException($"No Cart with id: {id} was found.");
        return cart;
    }

    public async Task<Cart> GetUserCart(int userId)
    {
        var userCart = await BaseCartQuery().FirstOrDefaultAsync(c => c.UserId == userId);
        if (userCart == null)
            throw new NotFoundException($"No Cart was found for user id: {userId}. try logging in");
        return userCart;
    }

    public async Task<Cart> GetCartForModifying(int userId)
    {
        var userCart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        if (userCart == null)
            throw new NotFoundException($"No Cart was found for user id: {userId}. try logging in");
        return userCart;
    }

    public async Task AddCartItemToCart(CartItem item)
    {
        await _context.CartItems.AddAsync(item);
    }

    public void RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem); 
    }
    
    public async Task ClearCart(int userId)
    {
        var userCart = await GetCartForModifying(userId);
        _context.CartItems.RemoveRange(userCart.CartItems);
    }
}
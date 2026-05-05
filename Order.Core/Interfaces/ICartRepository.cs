using Order.Core.Entities;
using Order.Core.Enums;

namespace Order.Core.Interfaces;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<List<Cart>> GetCartsForAdmin();
    Task<Cart> GetCartWithItemsById(int id);
    Task<Cart> GetUserCart(int userId);
    Task<Cart> GetCartForModifying(int userId);
    Task AddCartItemToCart(CartItem item);
    void RemoveCartItem(CartItem cartItem);
    Task ClearCart(int userId);
}
using Order.Application.Dtos.Cart;

namespace Order.Application.Interfaces;

public interface ICartService
{
    Task AddToCart(int userId, AddItemToCartDto request);
    Task<List<CartDto>> GetAllCarts();
    Task<CartDto> GetCartById(int cartId);
    Task<CartDto> GetUserCart(int userId);
    Task RemoveFromCart(int userId, int cartItemId);
    Task UpdateQuantity(int userId, UpdateCartItemQuantityDto request);
}
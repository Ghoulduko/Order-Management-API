using AutoMapper;
using Order.Application.Dtos.Cart;
using Order.Application.Dtos.Product;
using Order.Application.Interfaces;
using Order.Core.Entities;
using Order.Core.Enums;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Carts;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, IProductService productService, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<List<CartDto>> GetAllCarts()
    {
        return _mapper.Map<List<CartDto>>(await _cartRepository.GetCartsForAdmin());
    }

    public async Task<CartDto> GetCartById(int cartId)
    {
        return _mapper.Map<CartDto>(await _cartRepository.GetCartWithItemsById(cartId));
    }

    public async Task<CartDto> GetUserCart(int userId)
    {
        return _mapper.Map<CartDto>(await _cartRepository.GetUserCart(userId));
    }

    // Helper method
    private void ValidateStock(int productStock, int requestQuantity, int inCartItemQuantity)
    {
        if (inCartItemQuantity + requestQuantity > productStock)
            throw new InsufficientStockException("Insufficient stock");
    }

    public async Task AddToCart(int userId, AddItemToCartDto req)
    {
        var product = await _productService.GetById(req.ProductId);
        
        var cart = await _cartRepository.GetCartForModifying(userId);
        var itemInCart = cart.CartItems.FirstOrDefault(i => i.ProductId == product.Id);
        int currentQuantity = itemInCart?.Quantity ?? 0;
        
        ValidateStock(product.Stock, req.Quantity, currentQuantity);

        await AddOrUpdateCartItem(cart, itemInCart, req);
        await _cartRepository.SaveAsync();
    }

    // Helper method
    private async Task AddOrUpdateCartItem(Cart cart, CartItem? cartItem, AddItemToCartDto req)
    {
        if (cartItem != null)
        {
            cartItem.Quantity += req.Quantity;
        }
        else
        {
            var itemToAdd = _mapper.Map<CartItem>(req);
            itemToAdd.CartId = cart.Id;
            await _cartRepository.AddCartItemToCart(itemToAdd);
        }
    }

    public async Task RemoveFromCart(int userId, int cartItemId)
    {
        var cart = await _cartRepository.GetCartForModifying(userId);
        var cartItem = cart.CartItems.FirstOrDefault(c => c.Id == cartItemId);
        if (cartItem == null)
            throw new NotFoundException($"No Cart item with id: {cartItemId} was found.");
        _cartRepository.RemoveCartItem(cartItem);
        await _cartRepository.SaveAsync();
    }
    
    //Helper method
    private void ValidateQuantityUpdate(int productStock, int requestQuantity)
    {
        if (requestQuantity > productStock)
            throw new InsufficientStockException("Insufficient stock");
        if (requestQuantity < 1)
            throw new ArgumentException("Quantity Cannot be less than 1.");
    }

    public async Task UpdateQuantity(int userId, UpdateCartItemQuantityDto req)
    {
        var cart = await _cartRepository.GetCartForModifying(userId);
        var cartItem = cart.CartItems.FirstOrDefault(c => c.Id == req.CartItemId && c.CartId == cart.Id);
        if (cartItem == null)
            throw new NotFoundException("No changeable item found in cart.");
        
        var product = await _productService.GetById(cartItem.ProductId);
        
        switch(req.QuantityAction)
        {
            case UpdateCartItemQuantity.Increment:
                ValidateQuantityUpdate(product.Stock, cartItem.Quantity + 1);
                cartItem.Quantity += 1;
                break;
            case UpdateCartItemQuantity.Decrement:
                ValidateQuantityUpdate(product.Stock, cartItem.Quantity - 1);
                cartItem.Quantity -= 1;
                break;
            default:
                throw new ArgumentException("Wrong Quantity Action Detected.");
        };

        await _cartRepository.SaveAsync();
    }
}
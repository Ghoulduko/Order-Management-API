using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos.Cart;
using Order.Application.Interfaces;

namespace Order_Management_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IConfiguration _configuration;
    private readonly int UserId;

    public CartController(ICartService cartService, IConfiguration configuration)
    {
        _cartService = cartService;
        _configuration = configuration;
        UserId = _configuration.GetValue<int>("UserId");
    }

    [HttpPost("AddToCart")]
    public async Task<Ok<string>> AddToCart([FromBody] AddItemToCartDto req)
    {
        await _cartService.AddToCart(UserId, req);
        return TypedResults.Ok("Successfully added item to cart");
    }

    [HttpGet("GetAllCarts")]
    public async Task<Ok<List<CartDto>>> GetAllCarts()
    {
        return TypedResults.Ok(await _cartService.GetAllCarts());
    }

    [HttpGet("GetCartById/{cartId}")]
    public async Task<Ok<CartDto>> GetCartById(int cartId)
    {
        return TypedResults.Ok(await _cartService.GetCartById(cartId));
    }

    [HttpGet("GetUserCart")]
    public async Task<Ok<CartDto>> GetUserCart()
    {
        return TypedResults.Ok(await _cartService.GetUserCart(UserId));
    }

    [HttpPatch("UpdateQuantity")]
    public async Task<Ok<string>> UpdateQuantity([FromBody] UpdateCartItemQuantityDto req)
    {
        await _cartService.UpdateQuantity(UserId, req);
        return TypedResults.Ok("Successfully updated cart");
    }

    [HttpDelete("RemoveFromCart/{cartItemId}")]
    public async Task<Ok<string>> RemoveFromCart(int cartItemId)
    {
        await _cartService.RemoveFromCart(UserId, cartItemId);
        return TypedResults.Ok("Successfully removed item from cart");
    }
}
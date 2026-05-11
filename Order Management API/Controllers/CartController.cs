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

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("AddToCart")]
    public async Task<Ok<string>> AddToCart([FromBody] AddItemToCartDto req)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new Exception("You need to login first");
        await _cartService.AddToCart(int.Parse(userId), req);
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
        var userId = User.FindFirst("UserId")?.Value ?? throw new Exception("You need to login first");
        return TypedResults.Ok(await _cartService.GetUserCart(int.Parse(userId)));
    }

    [HttpPatch("UpdateQuantity")]
    public async Task<Ok<string>> UpdateQuantity([FromBody] UpdateCartItemQuantityDto req)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new Exception("You need to login first");
        await _cartService.UpdateQuantity(int.Parse(userId), req);
        return TypedResults.Ok("Successfully updated cart");
    }

    [HttpDelete("RemoveFromCart/{cartItemId}")]
    public async Task<Ok<string>> RemoveFromCart(int cartItemId)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new Exception("You need to login first");
        await _cartService.RemoveFromCart(int.Parse(userId), cartItemId);
        return TypedResults.Ok("Successfully removed item from cart");
    }
}
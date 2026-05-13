using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<Ok<string>> AddToCart([FromBody] AddItemToCartDto req)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        await _cartService.AddToCart(int.Parse(userId), req);
        return TypedResults.Ok("Successfully added item to cart");
    }

    [HttpGet("GetAllCarts")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<List<CartDto>>> GetAllCartsAdmin()
    {
        return TypedResults.Ok(await _cartService.GetAllCarts());
    }

    [HttpGet("GetCartById/{cartId}")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<CartDto>> GetCartByIdAdmin(int cartId)
    {
        return TypedResults.Ok(await _cartService.GetCartById(cartId));
    }

    [HttpGet("GetUserCart")]
    [Authorize]
    public async Task<Ok<CartDto>> GetUserCart()
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        return TypedResults.Ok(await _cartService.GetUserCart(int.Parse(userId)));
    }

    [HttpPatch("UpdateQuantity")]
    [Authorize]
    public async Task<Ok<string>> UpdateQuantity([FromBody] UpdateCartItemQuantityDto req)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        await _cartService.UpdateQuantity(int.Parse(userId), req);
        return TypedResults.Ok("Successfully updated cart");
    }

    [HttpDelete("RemoveFromCart/{cartItemId}")]
    [Authorize]
    public async Task<Ok<string>> RemoveFromCart(int cartItemId)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        await _cartService.RemoveFromCart(int.Parse(userId), cartItemId);
        return TypedResults.Ok("Successfully removed item from cart");
    }
}
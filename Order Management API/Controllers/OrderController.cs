using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos.Order;
using Order.Application.Interfaces;

namespace Order_Management_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("Order")]
    [Authorize]
    public async Task<Ok<string>> AddOrder([FromBody] AddOrderDto order)
    {
        var userEmail = User.FindFirst("Email")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        await _orderService.Add(order.PaymentMethod, int.Parse(userId), userEmail);
        return TypedResults.Ok("Your order was Successful!");
    }

    [HttpGet("GetAllUserOrders")]
    [Authorize]
    public async Task<Ok<List<CustomerOrderDto>>> GetAllOrdersUser()
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        var orders = await _orderService.GetAllUserOrders(int.Parse(userId));
        return TypedResults.Ok(orders);
    }

    [HttpGet("GetAllOrdersAdmin")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<List<CustomerOrderDto>>> GetAllOrdersAdmin()
    {
        var orders = await _orderService.GetAll();
        return TypedResults.Ok(orders);
    }

    [HttpGet("GetByIdForUser/{id}")]
    [Authorize]
    public async Task<Ok<CustomerOrderDto>> GetById(int id)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        var order = await _orderService.GetByIdForUser(id, int.Parse(userId));
        return TypedResults.Ok(order);
    }
    
    [HttpGet("GetByIdForAdmin/{id}")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<CustomerOrderDto>> GetByIdAdmin(int id)
    {
        var order = await _orderService.GetByIdForAdmin(id);
        return TypedResults.Ok(order);
    }

    [HttpDelete("CancelOrderById/{id}")]
    [Authorize]
    public async Task<Ok<string>> CancelOrder(int id)
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        
        await _orderService.CancelOrderById(id, int.Parse(userId));
        return TypedResults.Ok("Successfully canceled the order");
    }
}
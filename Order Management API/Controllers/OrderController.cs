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
    private readonly IConfiguration _configuration;
    private readonly int userId;
    public OrderController(IOrderService orderService, IConfiguration configuration)
    {
        _orderService = orderService;
        _configuration = configuration;
        userId = _configuration.GetValue<int>("UserId");
    }

    [HttpPost("Order")]
    public async Task<Ok<string>> AddOrder([FromBody] AddOrderDto order)
    {
        await _orderService.Add(order.PaymentMethod, userId);
        return TypedResults.Ok("Your order was Successful!");
    }

    [HttpGet("GetAllUserOrders")]
    public async Task<Ok<List<CustomerOrderDto>>> GetAllUserOrders()
    {
        var orders = await _orderService.GetAllUserOrders(userId);
        return TypedResults.Ok(orders);
    }

    [HttpGet("GetAllOrdersAdmin")]
    public async Task<Ok<List<CustomerOrderDto>>> GetAllOrders()
    {
        var orders = await _orderService.GetAll();
        return TypedResults.Ok(orders);
    }

    [HttpGet("GetByIdUser/{id}")]
    public async Task<Ok<CustomerOrderDto>> GetById(int id)
    {
        var order = await _orderService.GetByIdForUser(id, userId);
        return TypedResults.Ok(order);
    }
    
    [HttpGet("GetByIdForAdmin/{id}")]
    public async Task<Ok<CustomerOrderDto>> GetByIdAdmin(int id)
    {
        var order = await _orderService.GetByIdForAdmin(id);
        return TypedResults.Ok(order);
    }

    [HttpDelete("CancelOrderById/{id}")]
    public async Task<Ok<string>> CancelOrder(int id)
    {
        DeleteOrderDto req = new DeleteOrderDto
        {
            OrderId = id,
            UserId = userId
        };
        
        await _orderService.CancelOrderById(req);
        return TypedResults.Ok("Successfully deleted the order");
    }
}
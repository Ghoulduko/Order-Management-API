using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos.Payment;
using Order.Application.Interfaces;

namespace Order_Management_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;
    private readonly int userId;
    public PaymentController(IPaymentService paymentService, IConfiguration configuration)
    {
        _paymentService = paymentService;
        _configuration = configuration;
        userId = _configuration.GetValue<int>("UserId");
    }

    [HttpGet("GetAllPaymentsUser")]
    public async Task<Ok<List<PaymentDto>>> GetAll()
    {
        var payments = await _paymentService.GetAllUserPayments(userId);
        return TypedResults.Ok(payments);
    }

    [HttpGet("GetAllPaymentsAdmin")]
    public async Task<Ok<List<PaymentDto>>> GetAllAdmin()
    {
        var payments = await _paymentService.GetAll();
        return TypedResults.Ok(payments);
    }

    [HttpGet("GetById/{id}")]
    public async Task<Ok<PaymentDto>> GetById(int id)
    {
        var payment = await _paymentService.GetById(id);
        return TypedResults.Ok(payment);
    }

    [HttpDelete("DeleteById/{id}")]
    public async Task<Ok<string>> DeleteById(int id)
    {
        await _paymentService.Delete(id);
        return TypedResults.Ok("Payment deleted successfully");
    }
    
}
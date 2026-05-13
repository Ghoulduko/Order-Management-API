using Microsoft.AspNetCore.Authorization;
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
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("GetAllPaymentsUser")]
    [Authorize]
    public async Task<Ok<List<PaymentDto>>> GetAll()
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        var payments = await _paymentService.GetAllUserPayments(int.Parse(userId));
        return TypedResults.Ok(payments);
    }

    [HttpGet("GetAllPaymentsAdmin")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<List<PaymentWithUserDto>>> GetAllAdmin()
    {
        var payments = await _paymentService.GetAll();
        return TypedResults.Ok(payments);
    }

    [HttpGet("GetById/{id}")]
    [Authorize]
    public async Task<Ok<PaymentDto>> GetById(int id)
    {
        var payment = await _paymentService.GetById(id);
        return TypedResults.Ok(payment);
    }
}
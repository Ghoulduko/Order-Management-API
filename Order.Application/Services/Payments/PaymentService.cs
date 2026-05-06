using AutoMapper;
using Order.Application.Dtos.Payment;
using Order.Application.Interfaces;
using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Payments;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IMapper _mapper;
    
    public PaymentService(IPaymentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task Add(Payment req)
    {
        await _repository.AddAsync(req);
    }

    public async Task<PaymentDto> GetById(int id)
    {
        var payment = await _repository.GetByIdAsync(id);
        if (payment == null)
            throw new NotFoundException("Payment not found");
        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task<List<PaymentDto>> GetAll()
    {
        var payments = await _repository.GetAllAsync();
        return _mapper.Map<List<PaymentDto>>(payments);
    }

    public async Task<List<PaymentDto>> GetAllUserPayments(int userId)
    {
        var payments = await _repository.GetAllUserPayments(userId);
        return _mapper.Map<List<PaymentDto>>(payments);
    }
}
using AutoMapper;
using Order.Application.Dtos.Order;
using Order.Application.Dtos.Payment;
using Order.Application.Interfaces;
using Order.Application.Services.Products;
using Order.Core.Entities;
using Order.Core.Enums;
using Order.Core.Interfaces;

namespace Order.Application.Services.Orders;

public class OrderService  : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ICartRepository _cartRepository;
    private readonly IPaymentFactory _paymentFactory;
    private readonly InventoryService _inventoryService;
    private readonly List<IOrderObserver> _observers;
    private readonly IMapper _mapper;
    
    public OrderService(IOrderRepository repository, ICartRepository cartRepository, IPaymentFactory paymentFactory, InventoryService inventoryService, IEnumerable<IOrderObserver> observers, IMapper mapper)
    {
        _repository = repository;
        _cartRepository = cartRepository;
        _paymentFactory = paymentFactory;
        _inventoryService = inventoryService;
        _observers = observers.ToList();
        _mapper = mapper;
    }

    public async Task Add(PaymentMethod paymentMethod, int userId, string userEmail)
    {
        var userCart = await _cartRepository.GetUserCart(userId);

        var chosenPaymentMethod = _paymentFactory.ChoosePaymentMethod(paymentMethod);
        
        if (userCart.CartItems == null || userCart.CartItems.Count == 0)
            throw new InvalidOperationException("No items found in cart to order");

        decimal totalPrice = userCart.CartItems.Sum(i => i.Product.Price * i.Quantity);

        var orderItems = userCart.CartItems.Select(i => new OrderItem
        {
            ProductId = i.Product.Id,
            Quantity = i.Quantity,
        }).ToList();
        
        var order = new CustomerOrder
        {
            OrderItems = orderItems,
            Total =  totalPrice,
            CreatedAt = DateTime.Now,
            IsCanceled =  false,
            UserId = userId,
            Payment = new Payment {
                PaymentMethod =  paymentMethod,
                Amount = totalPrice,
                UserId = userId,
                PaidAt = DateTime.Now,
            }
        };

        chosenPaymentMethod.ProcessPayment();
        await _repository.AddAsync(order);
        await _cartRepository.ClearCart(userId);
        
        foreach (var orderItem in orderItems)
        {
            await _inventoryService.DecrementStock(orderItem.ProductId, orderItem.Quantity);
        }
        
        await _repository.SaveAsync();

        var orderDto = _mapper.Map<CustomerOrderDto>(order);
        foreach (var observer in _observers)
        {
            await observer.OnOrderPlaced(orderDto, userEmail);
        }
    }

    public async Task<CustomerOrderDto> GetByIdForUser(int orderId, int userId)
    {
        return _mapper.Map<CustomerOrderDto>(await _repository.GetOrderByIdForUser(orderId, userId));
    }
    
    public async Task<CustomerOrderDto> GetByIdForAdmin(int orderId)
    {
        return _mapper.Map<CustomerOrderDto>(await _repository.GetOrderById(orderId));
    }

    public async Task<List<CustomerOrderDto>> GetAll()
    {
        return _mapper.Map<List<CustomerOrderDto>>(await _repository.GetAllOrders());
    }

    public async Task<List<CustomerOrderDto>> GetAllUserOrders(int userId)
    {
        return _mapper.Map<List<CustomerOrderDto>>(await _repository.GetAllUserOrders(userId));
    }

    public async Task CancelOrderById(int orderId, int userId)
    {
        var order = await _repository.GetOrderById(orderId);
        
        if (order.UserId != userId || order.IsCanceled)
            throw new UnauthorizedAccessException("Order not found.");
        
        order.IsCanceled = true;
        await _repository.SaveAsync();
    }
}
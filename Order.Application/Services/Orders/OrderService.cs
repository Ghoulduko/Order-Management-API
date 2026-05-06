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
    private readonly InventoryService _inventoryService;
    private readonly List<IOrderObserver> _observers;
    private readonly IMapper _mapper;
    
    public OrderService(IOrderRepository repository, ICartRepository cartRepository, InventoryService inventoryService, IEnumerable<IOrderObserver> observers, IMapper mapper)
    {
        _repository = repository;
        _cartRepository = cartRepository;
        _inventoryService = inventoryService;
        _observers = observers.ToList();
        _mapper = mapper;
    }

    public async Task Add(PaymentMethod paymentMethod, int userId)
    {
        var userCart = await _cartRepository.GetUserCart(userId);
        
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
            UserId = userId,
            Payment = new Payment {
                PaymentMethod =  paymentMethod,
                Amount = totalPrice,
                UserId = userId,
                PaidAt = DateTime.Now,
            }
        };
        
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
            await observer.OnOrderPlaced(orderDto);
        }
    }

    public async Task<CustomerOrderDto> GetByIdForUser(int id, int userId)
    {
        var order = await _repository.GetOrderById(id);
        if (order.UserId != userId) 
            throw new UnauthorizedAccessException("Order not found.");
        return _mapper.Map<CustomerOrderDto>(order);
    }
    
    public async Task<CustomerOrderDto> GetByIdForAdmin(int id)
    {
        var order = await _repository.GetOrderById(id);
        return _mapper.Map<CustomerOrderDto>(order);
    }

    public async Task<List<CustomerOrderDto>> GetAll()
    {
        var orders = await _repository.GetAllOrders();
        return _mapper.Map<List<CustomerOrderDto>>(orders);
    }

    public async Task<List<CustomerOrderDto>> GetAllUserOrders(int userId)
    {
        var orders = await _repository.GetAllUserOrders(userId);
        return _mapper.Map<List<CustomerOrderDto>>(orders);
    }

    public async Task DeleteById(DeleteOrderDto req)
    {
        var order = await _repository.GetOrderById(req.OrderId);
        
        if (order.UserId != req.UserId)
            throw new UnauthorizedAccessException("Order not found.");
        
        await _repository.DeleteAsync(order);
    }
}
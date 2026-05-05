using Order.Application.Dtos.Order;
using Order.Core.Entities;

namespace Order.Application.Interfaces;

public interface IOrderObserver
{
    Task OnOrderPlaced(CustomerOrderDto customerOrder);
}
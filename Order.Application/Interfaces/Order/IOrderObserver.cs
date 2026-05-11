using Order.Application.Dtos.Order;
using Order.Core.Entities;

namespace Order.Application.Interfaces;

public interface IOrderObserver
{
    Task OnOrderPlaced(CustomerOrderDto customerOrder, string email);
    Task OnLogin(string email, string username);
}
using AutoMapper;
using Order.Application.Dtos.Cart;
using Order.Application.Dtos.Order;
using Order.Application.Dtos.Payment;
using Order.Application.Dtos.Product;
using Order.Application.Dtos.Role;
using Order.Application.Dtos.User;
using Order.Core.Entities;

namespace Order.Application.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Order Mapping
        CreateMap<CustomerOrderDto, CustomerOrder>().ReverseMap();
        CreateMap<AddOrderDto, CustomerOrder>().ReverseMap();
        CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        CreateMap<OrderItem, CartItem>().ReverseMap();
        
        // Cart Mapping
        CreateMap<CartDto, Cart>().ReverseMap();
        CreateMap<CartItemDto, CartItem>().ReverseMap();
        CreateMap<CartItem, AddItemToCartDto>().ReverseMap();
        CreateMap<CartDto, Cart>().ReverseMap();
        
        // Product Mapping
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<AddProductDto, Product>().ReverseMap();
        
        // Payment Mapping
        CreateMap<PaymentDto, Payment>().ReverseMap();
        CreateMap<AddPaymentDto, Payment>().ReverseMap();
        CreateMap<PaymentWithUserDto, Payment>().ReverseMap();
        
        // User Mapping
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<AddUserDto, User>().ReverseMap();
        
        // Role Mapping
        CreateMap<RoleDto, Role>().ReverseMap();
    }
}
using Microsoft.EntityFrameworkCore;
using Order_Management_API.Middlewares;
using Order.Application.Dtos.Product;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;
using Order.Application.Interfaces.Helper;
using Order.Application.Mapper;
using Order.Application.Services.Carts;
using Order.Application.Services.Notifications;
using Order.Application.Services.Orders;
using Order.Application.Services.Payments;
using Order.Application.Services.Products;
using Order.Application.Services.Users;
using Order.Core.Database;
using Order.Core.Entities;
using Order.Core.Entities.PaymentTypes;
using Order.Core.Helper;
using Order.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(i => 
    i.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Generic Repositories
builder.Services.AddScoped<IGenericRepository<CustomerOrder>, GenericRepository<CustomerOrder>>();
builder.Services.AddScoped<IGenericRepository<OrderItem>, GenericRepository<OrderItem>>();
builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
builder.Services.AddScoped<IGenericRepository<Payment>, GenericRepository<Payment>>();
builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IGenericRepository<Cart>, GenericRepository<Cart>>();
builder.Services.AddScoped<IGenericRepository<CartItem>, GenericRepository<CartItem>>();

// Order Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Cart Service
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Product Services
builder.Services.AddScoped<IProductService, ProductService>();

// Product InventoryService
builder.Services.AddScoped<InventoryService>();

// User Services
builder.Services.AddScoped<IUserService, UserService>();

// Payment Services
builder.Services.AddScoped<IPaymentFactory, PaymentFactory>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Order Observers
builder.Services.AddTransient<IOrderObserver, EmailNotificationObserver>();

// Validators
builder.Services.AddScoped<IValidator<AddProductDto>, AddProductValidator>();
builder.Services.AddScoped<IValidator<AddUserDto>, AddUserValidator>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
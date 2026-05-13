using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Order_Management_API.Middlewares;
using Order.Application.Dtos.Product;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;
using Order.Application.Interfaces.Authentication;
using Order.Application.Interfaces.Helper;
using Order.Application.Interfaces.Role;
using Order.Application.Mapper;
using Order.Application.Services.Authentication;
using Order.Application.Services.Carts;
using Order.Application.Services.Notifications;
using Order.Application.Services.Orders;
using Order.Application.Services.Payments;
using Order.Application.Services.Products;
using Order.Application.Services.Roles;
using Order.Application.Services.Users;
using Order.Core.Database;
using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Helper;
using Order.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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
builder.Services.AddScoped<IGenericRepository<Role>, GenericRepository<Role>>();

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Payment Services
builder.Services.AddScoped<IPaymentFactory, PaymentFactory>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Order Observers
builder.Services.AddTransient<IOrderObserver, EmailNotificationObserver>();

// Validators
builder.Services.AddScoped<IValidator<AddProductDto>, AddProductValidator>();
builder.Services.AddScoped<IValidator<AddUserDto>, AddUserValidator>();

// Authentication Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Email Service
builder.Services.AddTransient<EmailNotificationObserver>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

var jwtKey = builder.Configuration["JwtSecretKey"] ?? throw new JwtKeyNotFoundException("No JWT Secret Key was found");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://ltdluka.ge/",
            ValidAudience = "https://ltdluka.ge/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cyber Commerce API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
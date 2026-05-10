using Microsoft.EntityFrameworkCore;
using Order.Core.Entities;

namespace Order.Core.Database;

public class OrderDbContext : DbContext
{ 
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.CustomerOrder)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.CustomerOrderId);
    
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.CustomerOrder)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.CustomerOrderId);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<CustomerOrder> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
}
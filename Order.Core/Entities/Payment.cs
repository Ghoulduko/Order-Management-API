using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Order.Core.Enums;
using Order.Core.Interfaces;

namespace Order.Core.Entities;

[Table("Payments")]
public class Payment
{
    [Key]
    public int Id { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public int CustomerOrderId { get; set; }
    public CustomerOrder? CustomerOrder { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
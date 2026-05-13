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
    [Required]
    public required PaymentMethod PaymentMethod { get; set; }
    [Required]
    public required decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    [Required] // may cause an error
    public int CustomerOrderId { get; set; }
    public CustomerOrder? CustomerOrder { get; set; }
    [Required]
    public required int UserId { get; set; }
    public User? User { get; set; }
}
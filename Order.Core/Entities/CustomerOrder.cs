using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("Orders")]
public class CustomerOrder
{
    [Key]
    public int Id { get; set; }
    public List<OrderItem> OrderItems { get; set; }  = new List<OrderItem>();
    public decimal Total { get; set; }
    public Payment? Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
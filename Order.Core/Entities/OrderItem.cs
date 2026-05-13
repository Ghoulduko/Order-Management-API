using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("OrderItems")]
public class OrderItem
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public required int Quantity { get; set; }
    
    [Required]
    public int CustomerOrderId { get; set; }
    public CustomerOrder CustomerOrder { get; set; }
    
    [Required]
    public required int ProductId { get; set; }
    public Product Product { get; set; }
}
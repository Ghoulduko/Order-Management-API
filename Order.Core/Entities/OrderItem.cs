using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("OrderItems")]
public class OrderItem
{
    [Key]
    public int Id { get; set; }
    public int Quantity { get; set; }
    
    public int CustomerOrderId { get; set; }
    public CustomerOrder CustomerOrder { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
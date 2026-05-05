using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("Carts")]
public class Cart
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
    
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();

}
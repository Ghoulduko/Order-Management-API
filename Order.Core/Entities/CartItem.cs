using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("CartItems")]
public class CartItem
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required int Quantity { get; set; }

    [Required]
    public required int CartId { get; set; }
    public Cart? Cart { get; set; }

    [Required]
    public required int ProductId { get; set; }
    public Product? Product { get; set; }
}
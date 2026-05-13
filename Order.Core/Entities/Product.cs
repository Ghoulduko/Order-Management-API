using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("Products")]
public class Product
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
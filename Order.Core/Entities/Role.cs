using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("Roles")]
public class Role
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public List<User> Users { get; set; } = new List<User>();
}
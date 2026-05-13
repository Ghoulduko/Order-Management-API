using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Core.Entities;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Length(3, 10)]
    public required string Username { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    [Length(8, 22)]
    public required string Password { get; set; }
    [Required]
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public bool IsDeleted { get; set; } = false;
}
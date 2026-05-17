using System.ComponentModel.DataAnnotations;

namespace LoginApp.API.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

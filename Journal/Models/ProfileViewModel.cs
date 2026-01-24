using System.ComponentModel.DataAnnotations;

namespace Journal.Models;

public class ProfileViewModel
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [StringLength(500)]
    public string Bio { get; set; } = string.Empty;

    public bool IsDarkMode { get; set; }
    public bool IsCompactView { get; set; }
}

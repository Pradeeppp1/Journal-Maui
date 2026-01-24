namespace Journal.Models;

public class ProfileDisplayModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public bool IsDarkMode { get; set; }
    public bool IsCompactView { get; set; }
    public string Initials => string.IsNullOrWhiteSpace(FullName) 
        ? "U" 
        : string.Join("", FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0])).ToUpper();
}

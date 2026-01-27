using Journal.Common;
using Journal.Models;

namespace Journal.Services;

public interface IAuthService
{
    Task<ServiceResult<UserDisplayModel>> LoginAsync(string username, string password);
    Task<ServiceResult<UserDisplayModel>> RegisterAsync(RegisterViewModel model);
    Task LogoutAsync();
    Task<UserDisplayModel?> GetCurrentUserAsync();
    bool IsAuthenticated { get; }
    int? CurrentUserId { get; }
    
    // Session & PIN
    bool IsSessionLocked { get; }
    void UnlockSession();
    Task<bool> HasPinAsync();
    Task<bool> VerifyPinAsync(string pin);
    Task<bool> SetPinAsync(string pin);
}

public class UserDisplayModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

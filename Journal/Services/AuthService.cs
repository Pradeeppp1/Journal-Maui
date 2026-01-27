using Journal.Common;
using Journal.Data;
using Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;

namespace Journal.Services;

public class AuthService : IAuthService
{
    private readonly JournalDbContext _context;
    private const string SessionKey = "current_user_id";

    public AuthService(JournalDbContext context)
    {
        _context = context;
    }

    public bool IsAuthenticated => CurrentUserId.HasValue;

    public int? CurrentUserId 
    {
        get 
        {
            var id = Preferences.Default.Get(SessionKey, -1);
            return id == -1 ? null : id;
        }
    }

    private bool _isSessionLocked = true;
    public bool IsSessionLocked => IsAuthenticated && _isSessionLocked;

    public void UnlockSession()
    {
        _isSessionLocked = false;
    }

    public async Task<bool> HasPinAsync()
    {
        var id = CurrentUserId;
        if (!id.HasValue) return false;

        var user = await _context.UserProfiles.FindAsync(id.Value);
        return user != null && !string.IsNullOrEmpty(user.Pin);
    }

    public async Task<bool> VerifyPinAsync(string pin)
    {
        var id = CurrentUserId;
        if (!id.HasValue) return false;

        var user = await _context.UserProfiles.FindAsync(id.Value);
        if (user == null || user.Pin != pin) return false;
        
        UnlockSession();
        return true;
    }

    public async Task<bool> SetPinAsync(string pin)
    {
        var id = CurrentUserId;
        if (!id.HasValue) return false;

        var user = await _context.UserProfiles.FindAsync(id.Value);
        if (user == null) return false;

        user.Pin = pin;
        await _context.SaveChangesAsync();
        UnlockSession(); // Setting a PIN implies we are now effectively using it/active
        return true;
    }

    public async Task<ServiceResult<UserDisplayModel>> LoginAsync(string username, string password)
    {
        try
        {
            var user = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
                return ServiceResult<UserDisplayModel>.FailureResult("Invalid username or password");

            Preferences.Default.Set(SessionKey, user.Id);
            UnlockSession(); // Login with password unlocks session

            return ServiceResult<UserDisplayModel>.SuccessResult(MapToDisplayModel(user));
        }
        catch (Exception ex)
        {
            return ServiceResult<UserDisplayModel>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<UserDisplayModel>> RegisterAsync(RegisterViewModel model)
    {
        try
        {
            if (await _context.UserProfiles.AnyAsync(u => u.Username == model.Username))
                return ServiceResult<UserDisplayModel>.FailureResult("Username already exists");

            var user = new UserProfile
            {
                FullName = model.FullName,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                IsDarkMode = false,
                IsCompactView = false
            };

            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();

            Preferences.Default.Set(SessionKey, user.Id);
            UnlockSession(); // Registration unlocks session

            return ServiceResult<UserDisplayModel>.SuccessResult(MapToDisplayModel(user));
        }
        catch (Exception ex)
        {
            return ServiceResult<UserDisplayModel>.FailureResult(ex.Message);
        }
    }

    public Task LogoutAsync()
    {
        Preferences.Default.Remove(SessionKey);
        _isSessionLocked = true;
        return Task.CompletedTask;
    }

    public async Task<UserDisplayModel?> GetCurrentUserAsync()
    {
        var id = CurrentUserId;
        if (!id.HasValue) return null;

        var user = await _context.UserProfiles.FindAsync(id.Value);
        return user == null ? null : MapToDisplayModel(user);
    }

    private UserDisplayModel MapToDisplayModel(UserProfile user)
    {
        return new UserDisplayModel
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email
        };
    }
}

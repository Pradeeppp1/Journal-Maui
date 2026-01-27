using Journal.Common;
using Journal.Data;
using Journal.Entities;
using Journal.Models;
using Microsoft.EntityFrameworkCore;

namespace Journal.Services;

public class ProfileService : IProfileService
{
    private readonly JournalDbContext _context;
    private readonly IAuthService _authService;

    public ProfileService(JournalDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    private int CurrentUserId => _authService.CurrentUserId ?? -1;

    public async Task<ServiceResult<ProfileDisplayModel>> GetProfileAsync()
    {
        try
        {
            var profile = await _context.UserProfiles.FindAsync(CurrentUserId);
            if (profile == null)
            {
                return ServiceResult<ProfileDisplayModel>.FailureResult("Profile not found");
            }

            return ServiceResult<ProfileDisplayModel>.SuccessResult(MapToDisplayModel(profile));
        }
        catch (Exception ex)
        {
            return ServiceResult<ProfileDisplayModel>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<ProfileDisplayModel>> UpdateProfileAsync(ProfileViewModel model)
    {
        try
        {
            var profile = await _context.UserProfiles.FindAsync(CurrentUserId);
            if (profile == null)
            {
                return ServiceResult<ProfileDisplayModel>.FailureResult("Profile not found");
            }

            profile.FullName = model.FullName;
            profile.Email = model.Email;
            profile.Username = model.Username;
            profile.Bio = model.Bio;
            profile.IsDarkMode = model.IsDarkMode;
            profile.IsCompactView = model.IsCompactView;

            await _context.SaveChangesAsync();
            return ServiceResult<ProfileDisplayModel>.SuccessResult(MapToDisplayModel(profile));
        }
        catch (Exception ex)
        {
            return ServiceResult<ProfileDisplayModel>.FailureResult(ex.Message);
        }
    }

    private static ProfileDisplayModel MapToDisplayModel(UserProfile profile)
    {
        return new ProfileDisplayModel
        {
            Id = profile.Id,
            FullName = profile.FullName,
            Email = profile.Email,
            Username = profile.Username,
            Bio = profile.Bio,
            ProfilePictureUrl = profile.ProfilePictureUrl,
            IsDarkMode = profile.IsDarkMode,
            IsCompactView = profile.IsCompactView
        };
    }
}

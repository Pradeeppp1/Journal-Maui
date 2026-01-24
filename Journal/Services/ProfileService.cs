using Journal.Common;
using Journal.Data;
using Journal.Entities;
using Journal.Models;
using Microsoft.EntityFrameworkCore;

namespace Journal.Services;

public class ProfileService : IProfileService
{
    private readonly JournalDbContext _context;

    public ProfileService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult<ProfileDisplayModel>> GetProfileAsync()
    {
        try
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync();
            if (profile == null)
            {
                // Create a default profile if none exists
                profile = new UserProfile
                {
                    FullName = "User",
                    Email = "",
                    Username = "user",
                    Bio = "New member",
                    IsDarkMode = false,
                    IsCompactView = false
                };
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
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
            var profile = await _context.UserProfiles.FirstOrDefaultAsync();
            if (profile == null)
            {
                profile = new UserProfile();
                _context.UserProfiles.Add(profile);
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

using Journal.Common;
using Journal.Models;

namespace Journal.Services;

public interface IProfileService
{
    Task<ServiceResult<ProfileDisplayModel>> GetProfileAsync();
    Task<ServiceResult<ProfileDisplayModel>> UpdateProfileAsync(ProfileViewModel model);
}

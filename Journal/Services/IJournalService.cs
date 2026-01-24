using Journal.Common;
using Journal.Models;

namespace Journal.Services;

public interface IJournalService
{
    Task<ServiceResult<List<JournalDisplayModel>>> GetAllEntriesAsync();
    Task<ServiceResult<JournalDisplayModel>> GetEntryByIdAsync(int id);
    Task<ServiceResult<JournalDisplayModel>> CreateEntryAsync(JournalViewModel model);
    Task<ServiceResult<JournalDisplayModel>> UpdateEntryAsync(int id, JournalViewModel model);
    Task<ServiceResult<bool>> DeleteEntryAsync(int id);
    Task<ServiceResult<List<JournalDisplayModel>>> SearchEntriesAsync(string searchTerm);
    Task<ServiceResult<List<JournalDisplayModel>>> GetEntriesByMoodAsync(string mood);
    Task<ServiceResult<int>> GetTotalEntriesCountAsync();
    Task<ServiceResult<int>> GetCurrentStreakAsync();
    Task<ServiceResult<int>> GetThisMonthEntriesCountAsync();
    Task<ServiceResult<bool>> HasEntryTodayAsync();
    Task<ServiceResult<AnalyticsDto>> GetAnalyticsAsync(DateTime? startDate, DateTime? endDate);
}

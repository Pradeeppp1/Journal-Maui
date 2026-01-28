using Journal.Common;
using Journal.Data;
using Journal.Entities;
using Journal.Models;
using Microsoft.EntityFrameworkCore;

namespace Journal.Services;

public class JournalService : IJournalService
{
    private readonly JournalDbContext _context;
    private readonly IAuthService _authService;

    public JournalService(JournalDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    private int CurrentUserId => _authService.CurrentUserId ?? -1;

    public async Task<ServiceResult<List<JournalDisplayModel>>> GetAllEntriesAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            var entries = await _context.JournalEntries
                .Where(e => e.UserId == CurrentUserId)
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new JournalDisplayModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Content = e.Content,
                    CreatedAt = e.CreatedAt,
                    Mood = e.Mood,
                    SecondaryMoods = e.SecondaryMoods,
                    Tags = e.Tags,
                    WordCount = e.WordCount,
                    CharacterCount = e.CharacterCount
                })
                .ToListAsync();
            return ServiceResult<List<JournalDisplayModel>>.SuccessResult(entries);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JournalDisplayModel>>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<JournalDisplayModel>> GetEntryByIdAsync(int id)
    {
        try
        {
            var entry = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);
            if (entry == null) return ServiceResult<JournalDisplayModel>.FailureResult("Entry not found");
            return ServiceResult<JournalDisplayModel>.SuccessResult(MapToDisplayModel(entry));
        }
        catch (Exception ex)
        {
            return ServiceResult<JournalDisplayModel>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<JournalDisplayModel>> CreateEntryAsync(JournalViewModel model)
    {
        try
        {
            var today = DateTime.Today;
            var existingEntry = await _context.JournalEntries
                .AnyAsync(e => e.CreatedAt.Date == today && e.UserId == CurrentUserId);

            if (existingEntry)
            {
                return ServiceResult<JournalDisplayModel>.FailureResult("You have already created an entry for today. You can only create one entry per day.");
            }

            var entry = new JournalEntry
            {
                Title = model.Title,
                Content = model.Content,
                Mood = model.Mood,
                SecondaryMoods = model.SecondaryMoods,
                Tags = model.Tags,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                WordCount = CountWords(model.Content),
                CharacterCount = model.Content.Length,
                UserId = CurrentUserId
            };

            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
            return ServiceResult<JournalDisplayModel>.SuccessResult(MapToDisplayModel(entry));
        }
        catch (Exception ex)
        {
            return ServiceResult<JournalDisplayModel>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<JournalDisplayModel>> UpdateEntryAsync(int id, JournalViewModel model)
    {
        try
        {
            var entry = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);
            if (entry == null) return ServiceResult<JournalDisplayModel>.FailureResult("Entry not found");

            entry.Title = model.Title;
            entry.Content = model.Content;
            entry.Mood = model.Mood;
            entry.SecondaryMoods = model.SecondaryMoods;
            entry.Tags = model.Tags;
            entry.UpdatedAt = DateTime.Now;
            entry.WordCount = CountWords(model.Content);
            entry.CharacterCount = model.Content.Length;

            _context.JournalEntries.Update(entry);
            await _context.SaveChangesAsync();
            return ServiceResult<JournalDisplayModel>.SuccessResult(MapToDisplayModel(entry));
        }
        catch (Exception ex)
        {
            return ServiceResult<JournalDisplayModel>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<bool>> DeleteEntryAsync(int id)
    {
        try
        {
            var entry = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);
            if (entry == null) return ServiceResult<bool>.FailureResult("Entry not found");

            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<List<JournalDisplayModel>>> SearchEntriesAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllEntriesAsync();

            var entries = await _context.JournalEntries
                .Where(e => e.UserId == CurrentUserId && 
                           (e.Title.Contains(searchTerm) ||
                            e.Content.Contains(searchTerm) ||
                            e.Tags.Contains(searchTerm)))
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new JournalDisplayModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Content = e.Content,
                    CreatedAt = e.CreatedAt,
                    Mood = e.Mood,
                    SecondaryMoods = e.SecondaryMoods,
                    Tags = e.Tags,
                    WordCount = e.WordCount,
                    CharacterCount = e.CharacterCount
                })
                .ToListAsync();
            return ServiceResult<List<JournalDisplayModel>>.SuccessResult(entries);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JournalDisplayModel>>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<List<JournalDisplayModel>>> GetEntriesByMoodAsync(string mood)
    {
        try
        {
            var entries = await _context.JournalEntries
                .Where(e => e.Mood == mood && e.UserId == CurrentUserId)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new JournalDisplayModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Content = e.Content,
                    CreatedAt = e.CreatedAt,
                    Mood = e.Mood,
                    SecondaryMoods = e.SecondaryMoods,
                    Tags = e.Tags,
                    WordCount = e.WordCount,
                    CharacterCount = e.CharacterCount
                })
                .ToListAsync();
            return ServiceResult<List<JournalDisplayModel>>.SuccessResult(entries);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JournalDisplayModel>>.FailureResult(ex.Message);
        }

    }

    public async Task<ServiceResult<List<JournalDisplayModel>>> GetEntriesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var entries = await _context.JournalEntries
                .Where(e => e.UserId == CurrentUserId && e.CreatedAt.Date >= startDate.Date && e.CreatedAt.Date <= endDate.Date)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new JournalDisplayModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Content = e.Content,
                    CreatedAt = e.CreatedAt,
                    Mood = e.Mood,
                    SecondaryMoods = e.SecondaryMoods,
                    Tags = e.Tags,
                    WordCount = e.WordCount,
                    CharacterCount = e.CharacterCount
                })
                .ToListAsync();
            return ServiceResult<List<JournalDisplayModel>>.SuccessResult(entries);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<JournalDisplayModel>>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<int>> GetTotalEntriesCountAsync()
    {
        try
        {
            var count = await _context.JournalEntries
                .CountAsync(e => e.UserId == CurrentUserId);
            return ServiceResult<int>.SuccessResult(count);
        }
        catch (Exception ex)
        {
            return ServiceResult<int>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<int>> GetCurrentStreakAsync()
    {
        try
        {
            var entries = await _context.JournalEntries
                .Where(e => e.UserId == CurrentUserId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            if (entries.Count == 0)
                return ServiceResult<int>.SuccessResult(0);

            int streak = 0;
            DateTime currentDate = DateTime.Today;
            
            foreach (var entry in entries)
            {
                var entryDate = entry.CreatedAt.Date;
                if (entryDate == currentDate)
                {
                    streak++;
                    currentDate = currentDate.AddDays(-1);
                }
                else if (entryDate < currentDate)
                {
                    break;
                }
            }

            return ServiceResult<int>.SuccessResult(streak);
        }
        catch (Exception ex)
        {
            return ServiceResult<int>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<int>> GetThisMonthEntriesCountAsync()
    {
        try
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var count = await _context.JournalEntries
                .CountAsync(e => e.CreatedAt >= startOfMonth && e.UserId == CurrentUserId);
            return ServiceResult<int>.SuccessResult(count);
        }
        catch (Exception ex)
        {
            return ServiceResult<int>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<AnalyticsDto>> GetAnalyticsAsync(DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var query = _context.JournalEntries
                .Where(e => e.UserId == CurrentUserId)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.CreatedAt <= endDate.Value);

            var entries = await query.OrderBy(e => e.CreatedAt).ToListAsync();

            var analytics = new AnalyticsDto();

            if (entries.Count == 0)
                return ServiceResult<AnalyticsDto>.SuccessResult(analytics);

            // Mood Distribution
            analytics.MoodDistribution = entries
                .GroupBy(e => e.Mood)
                .ToDictionary(g => g.Key, g => g.Count());

            analytics.MostFrequentMood = analytics.MoodDistribution
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .FirstOrDefault() ?? "None";

            // Streak Logic
            var allEntries = await _context.JournalEntries
                .Where(e => e.UserId == CurrentUserId)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => e.CreatedAt.Date)
                .Distinct()
                .ToListAsync();

            if (allEntries.Count > 0)
            {
                // Current Streak
                int currentStreakCount = 0;
                DateTime currentCheckDate = DateTime.Today;
                
                // If the user hasn't posted today, the streak might still be active if they posted yesterday
                bool postedTodayOrYesterday = allEntries.Any(d => d == DateTime.Today || d == DateTime.Today.AddDays(-1));
                
                if (postedTodayOrYesterday)
                {
                    if (!allEntries.Any(d => d == DateTime.Today))
                        currentCheckDate = DateTime.Today.AddDays(-1);

                    foreach (var date in allEntries.Where(d => d <= currentCheckDate).OrderByDescending(d => d))
                    {
                        if (date == currentCheckDate)
                        {
                            currentStreakCount++;
                            currentCheckDate = currentCheckDate.AddDays(-1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                analytics.CurrentStreak = currentStreakCount;

                // Longest Streak
                int maxStreak = 0;
                int tempStreak = 0;
                DateTime? lastDate = null;

                foreach (var date in allEntries.OrderBy(d => d))
                {
                    if (lastDate == null || date == lastDate.Value.AddDays(1))
                    {
                        tempStreak++;
                    }
                    else
                    {
                        maxStreak = Math.Max(maxStreak, tempStreak);
                        tempStreak = 1;
                    }
                    lastDate = date;
                }
                analytics.LongestStreak = Math.Max(maxStreak, tempStreak);
            }

            // Missed Days (within the range if specified, otherwise last 30 days)
            var start = startDate ?? (entries.Any() ? entries.Min(e => e.CreatedAt).Date : DateTime.Today.AddDays(-30));
            var end = endDate ?? DateTime.Today;
            var entryDates = entries.Select(e => e.CreatedAt.Date).Distinct().ToHashSet();

            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
            {
                if (!entryDates.Contains(d))
                {
                    analytics.MissedDays.Add(d);
                }
            }

            // Tag Usage
            var tags = entries
                .Where(e => !string.IsNullOrEmpty(e.Tags))
                .SelectMany(e => e.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

            analytics.TagUsage = tags
                .GroupBy(t => t)
                .OrderByDescending(g => g.Count())
                .ToDictionary(g => g.Key, g => g.Count());

            // Word Count Trends
            analytics.WordCountTrends = entries
                .GroupBy(e => e.CreatedAt.Date)
                .Select(g => new WordCountTrendPoint
                {
                    Date = g.Key,
                    AverageWordCount = g.Average(e => e.WordCount)
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Activity by Day of Week
            analytics.ActivityByDayOfWeek = entries
                .GroupBy(e => e.CreatedAt.DayOfWeek.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            analytics.ActivityByHour = entries
                .GroupBy(e => e.CreatedAt.Hour)
                .ToDictionary(g => g.Key, g => g.Count());

            // Activity Heatmap (Day of Week vs Week of Month)
            var rangeStart = startDate ?? (entries.Any() ? entries.Min(e => e.CreatedAt).Date : DateTime.Today.AddDays(-30));
            var rangeEnd = endDate ?? DateTime.Today;
            
            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            foreach (var day in days)
            {
                var dayPoints = new HeatMapPoint { Name = day };
                // Group by week (simply divide range into 4-5 weeks for visibility)
                for (int w = 0; w < 5; w++)
                {
                    var weekStart = rangeStart.AddDays(w * 7);
                    var weekEnd = weekStart.AddDays(6);
                    if (weekStart > rangeEnd) break;

                    int count = entries.Count(e => e.CreatedAt.DayOfWeek.ToString() == day && 
                                                 e.CreatedAt.Date >= weekStart.Date && 
                                                 e.CreatedAt.Date <= weekEnd.Date);
                    
                    dayPoints.Data.Add(new HeatMapData { X = $"W{w+1}", Y = count });
                }
                analytics.ActivityHeatMap.Add(dayPoints);
            }

            return ServiceResult<AnalyticsDto>.SuccessResult(analytics);
        }
        catch (Exception ex)
        {
            return ServiceResult<AnalyticsDto>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<byte[]>> ExportEntriesToPdfAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var entries = await _context.JournalEntries
                .Where(e => e.UserId == CurrentUserId && e.CreatedAt.Date >= startDate.Date && e.CreatedAt.Date <= endDate.Date)
                .OrderBy(e => e.CreatedAt)
                .ToListAsync();

            if (entries.Count == 0)
                return ServiceResult<byte[]>.FailureResult("No entries found in the selected date range.");

            // Basic text export formatted as "PDF content"
            // In a real scenario, we would use a library like QuestPDF or iText
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("JOURNAL EXPORT");
            sb.AppendLine($"Range: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
            sb.AppendLine("==========================================");
            sb.AppendLine();

            foreach (var entry in entries)
            {
                sb.AppendLine($"TITLE: {entry.Title}");
                sb.AppendLine($"DATE: {entry.CreatedAt:F}");
                sb.AppendLine($"MOOD: {entry.Mood} (Secondary: {entry.SecondaryMoods})");
                sb.AppendLine($"TAGS: {entry.Tags}");
                sb.AppendLine("------------------------------------------");
                // Remove HTML tags for the text export
                var plainText = System.Text.RegularExpressions.Regex.Replace(entry.Content, "<.*?>", string.Empty);
                sb.AppendLine(plainText);
                sb.AppendLine();
                sb.AppendLine("==========================================");
                sb.AppendLine();
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return ServiceResult<byte[]>.SuccessResult(bytes);
        }
        catch (Exception ex)
        {
            return ServiceResult<byte[]>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<bool>> HasEntryTodayAsync()
    {
        try
        {
            var today = DateTime.Today;
            var exists = await _context.JournalEntries
                .AnyAsync(e => e.CreatedAt.Date == today && e.UserId == CurrentUserId);
            return ServiceResult<bool>.SuccessResult(exists);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.FailureResult(ex.Message);
        }
    }

    private static JournalDisplayModel MapToDisplayModel(JournalEntry entry)
    {
        return new JournalDisplayModel
        {
            Id = entry.Id,
            Title = entry.Title,
            Content = entry.Content,
            CreatedAt = entry.CreatedAt,
            Mood = entry.Mood,
            SecondaryMoods = entry.SecondaryMoods,
            Tags = entry.Tags,
            WordCount = entry.WordCount,
            CharacterCount = entry.CharacterCount
        };
    }

    private int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

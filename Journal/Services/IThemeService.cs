namespace Journal.Services;

public interface IThemeService
{
    event Action? OnThemeChanged;
    Task SetThemeAsync(string theme);
    Task<string> GetThemeAsync();
    Task InitializeThemeAsync();
}

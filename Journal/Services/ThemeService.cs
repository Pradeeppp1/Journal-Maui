using Microsoft.JSInterop;

namespace Journal.Services;

public class ThemeService : IThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = "light";

    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetThemeAsync(string theme)
    {
        _currentTheme = theme;
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme);
        await ApplyThemeAsync(theme);
        OnThemeChanged?.Invoke();
    }

    public async Task<string> GetThemeAsync()
    {
        var theme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
        return theme ?? "light";
    }

    public async Task InitializeThemeAsync()
    {
        _currentTheme = await GetThemeAsync();
        await ApplyThemeAsync(_currentTheme);
    }

    private async Task ApplyThemeAsync(string theme)
    {
        await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "data-theme", theme);
    }
}

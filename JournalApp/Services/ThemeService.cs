using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Models;

namespace JournalApp.Services;

public class ThemeService
{
    private readonly JournalDbContext _context;
    public event EventHandler<bool>? ThemeChanged;

    public ThemeService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<bool> GetIsDarkModeAsync()
    {
        var settings = await GetOrCreateSettingsAsync();
        return settings.IsDarkMode;
    }

    public async Task SetThemeAsync(bool isDarkMode)
    {
        var settings = await GetOrCreateSettingsAsync();
        settings.IsDarkMode = isDarkMode;
        await _context.SaveChangesAsync();
        ThemeChanged?.Invoke(this, isDarkMode);
    }

    public async Task ToggleThemeAsync()
    {
        var currentTheme = await GetIsDarkModeAsync();
        await SetThemeAsync(!currentTheme);
    }

    private async Task<AppSettings> GetOrCreateSettingsAsync()
    {
        var settings = await _context.AppSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new AppSettings
            {
                IsPasswordEnabled = false,
                IsDarkMode = false,
                CurrentStreak = 0,
                LongestStreak = 0
            };
            _context.AppSettings.Add(settings);
            await _context.SaveChangesAsync();
        }
        return settings;
    }
}

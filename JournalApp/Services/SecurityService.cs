using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Models;

namespace JournalApp.Services;

public class SecurityService
{
    private readonly JournalDbContext _context;

    public SecurityService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SetPasswordAsync(string password)
    {
        var settings = await GetOrCreateSettingsAsync();
        settings.PasswordHash = HashPassword(password);
        settings.IsPasswordEnabled = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> VerifyPasswordAsync(string password)
    {
        var settings = await GetOrCreateSettingsAsync();

        if (!settings.IsPasswordEnabled || string.IsNullOrEmpty(settings.PasswordHash))
            return true;

        return settings.PasswordHash == HashPassword(password);
    }

    public async Task<bool> RemovePasswordAsync()
    {
        var settings = await GetOrCreateSettingsAsync();
        settings.PasswordHash = null;
        settings.IsPasswordEnabled = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsPasswordEnabledAsync()
    {
        var settings = await GetOrCreateSettingsAsync();
        return settings.IsPasswordEnabled;
    }

    public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
    {
        if (!await VerifyPasswordAsync(oldPassword))
            return false;

        await SetPasswordAsync(newPassword);
        return true;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
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

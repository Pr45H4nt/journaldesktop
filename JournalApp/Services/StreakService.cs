using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Models;

namespace JournalApp.Services;

public class StreakService
{
    private readonly JournalDbContext _context;

    public StreakService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<(int currentStreak, int longestStreak, List<DateTime> missedDays)> CalculateStreaksAsync()
    {
        var entries = await _context.JournalEntries
            .OrderBy(e => e.Date)
            .Select(e => e.Date.Date)
            .ToListAsync();

        if (!entries.Any())
            return (0, 0, new List<DateTime>());

        var currentStreak = CalculateCurrentStreak(entries);
        var longestStreak = CalculateLongestStreak(entries);
        var missedDays = CalculateMissedDays(entries);

        var settings = await GetOrCreateSettingsAsync();
        settings.CurrentStreak = currentStreak;
        settings.LongestStreak = Math.Max(longestStreak, settings.LongestStreak);
        await _context.SaveChangesAsync();

        return (currentStreak, settings.LongestStreak, missedDays);
    }

    private int CalculateCurrentStreak(List<DateTime> entries)
    {
        var today = DateTime.Today;
        var streak = 0;

        if (entries.Contains(today))
        {
            streak = 1;
            var checkDate = today.AddDays(-1);

            while (entries.Contains(checkDate))
            {
                streak++;
                checkDate = checkDate.AddDays(-1);
            }
        }
        else if (entries.Contains(today.AddDays(-1)))
        {
            streak = 1;
            var checkDate = today.AddDays(-2);

            while (entries.Contains(checkDate))
            {
                streak++;
                checkDate = checkDate.AddDays(-1);
            }
        }

        return streak;
    }

    private int CalculateLongestStreak(List<DateTime> entries)
    {
        if (!entries.Any())
            return 0;

        var longestStreak = 1;
        var currentStreak = 1;

        for (int i = 1; i < entries.Count; i++)
        {
            if ((entries[i] - entries[i - 1]).Days == 1)
            {
                currentStreak++;
                longestStreak = Math.Max(longestStreak, currentStreak);
            }
            else
            {
                currentStreak = 1;
            }
        }

        return longestStreak;
    }

    private List<DateTime> CalculateMissedDays(List<DateTime> entries)
    {
        if (!entries.Any())
            return new List<DateTime>();

        var missedDays = new List<DateTime>();
        var startDate = entries.Min();
        var endDate = DateTime.Today;

        for (var date = startDate; date < endDate; date = date.AddDays(1))
        {
            if (!entries.Contains(date))
            {
                missedDays.Add(date);
            }
        }

        return missedDays;
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

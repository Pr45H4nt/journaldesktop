using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Models;

namespace JournalApp.Services;

public class AnalyticsService
{
    private readonly JournalDbContext _context;

    public AnalyticsService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<MoodCategory, int>> GetMoodDistributionAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.JournalEntries.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        var entries = await query.ToListAsync();

        var distribution = new Dictionary<MoodCategory, int>
        {
            { MoodCategory.Positive, 0 },
            { MoodCategory.Neutral, 0 },
            { MoodCategory.Negative, 0 }
        };

        foreach (var entry in entries)
        {
            var moods = new List<string> { entry.PrimaryMood };
            if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                moods.Add(entry.SecondaryMood1);
            if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                moods.Add(entry.SecondaryMood2);

            foreach (var moodName in moods)
            {
                var mood = Mood.AllMoods.FirstOrDefault(m => m.Name == moodName);
                if (mood != null)
                {
                    distribution[mood.Category]++;
                }
            }
        }

        return distribution;
    }

    public async Task<Dictionary<string, int>> GetMoodFrequencyAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.JournalEntries.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        var entries = await query.ToListAsync();

        var frequency = new Dictionary<string, int>();

        foreach (var entry in entries)
        {
            var moods = new List<string> { entry.PrimaryMood };
            if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                moods.Add(entry.SecondaryMood1);
            if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                moods.Add(entry.SecondaryMood2);

            foreach (var mood in moods)
            {
                if (!frequency.ContainsKey(mood))
                    frequency[mood] = 0;
                frequency[mood]++;
            }
        }

        return frequency.OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public async Task<string?> GetMostFrequentMoodAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var frequency = await GetMoodFrequencyAsync(startDate, endDate);
        return frequency.FirstOrDefault().Key;
    }

    public async Task<Dictionary<string, double>> GetWordCountTrendsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.JournalEntries.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        var entries = await query.OrderBy(e => e.Date).ToListAsync();

        if (!entries.Any())
            return new Dictionary<string, double>();

        var monthlyAverages = entries
            .GroupBy(e => new { e.Date.Year, e.Date.Month })
            .Select(g => new
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Average = g.Average(e => e.WordCount)
            })
            .ToDictionary(x => x.Month, x => x.Average);

        return monthlyAverages;
    }

    public async Task<double> GetAverageWordCountAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.JournalEntries.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        var count = await query.CountAsync();
        if (count == 0)
            return 0;

        var total = await query.SumAsync(e => e.WordCount);
        return (double)total / count;
    }

    public async Task<Dictionary<string, int>> GetCategoryBreakdownAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.JournalEntries.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        var entries = await query.ToListAsync();

        return entries
            .Where(e => !string.IsNullOrEmpty(e.Category))
            .GroupBy(e => e.Category!)
            .ToDictionary(g => g.Key, g => g.Count())
            .OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}

using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Models;

namespace JournalApp.Services;

public class JournalService
{
    private readonly JournalDbContext _context;

    public JournalService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date)
    {
        var dateOnly = date.Date;
        return await _context.JournalEntries
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Date.Date == dateOnly);
    }

    public async Task<List<JournalEntry>> GetEntriesAsync(int skip = 0, int take = 10)
    {
        return await _context.JournalEntries
            .Include(e => e.Tags)
            .OrderByDescending(e => e.Date)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<JournalEntry>> GetEntriesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.JournalEntries
            .Include(e => e.Tags)
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<List<JournalEntry>> SearchEntriesAsync(string searchTerm)
    {
        return await _context.JournalEntries
            .Include(e => e.Tags)
            .Where(e => (e.Title != null && e.Title.Contains(searchTerm)) ||
                       e.Content.Contains(searchTerm))
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<List<JournalEntry>> FilterEntriesAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        List<string>? moods = null,
        List<string>? tags = null)
    {
        var query = _context.JournalEntries.Include(e => e.Tags).AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        if (moods != null && moods.Any())
        {
            query = query.Where(e => moods.Contains(e.PrimaryMood) ||
                                    (e.SecondaryMood1 != null && moods.Contains(e.SecondaryMood1)) ||
                                    (e.SecondaryMood2 != null && moods.Contains(e.SecondaryMood2)));
        }

        if (tags != null && tags.Any())
        {
            query = query.Where(e => e.Tags.Any(t => tags.Contains(t.Name)));
        }

        return await query.OrderByDescending(e => e.Date).ToListAsync();
    }

    public async Task<JournalEntry> CreateEntryAsync(JournalEntry entry)
    {
        entry.CreatedAt = DateTime.Now;
        entry.UpdatedAt = DateTime.Now;
        entry.Date = entry.Date.Date;
        entry.WordCount = CountWords(entry.Content);

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task<JournalEntry> UpdateEntryAsync(JournalEntry entry)
    {
        entry.UpdatedAt = DateTime.Now;
        entry.WordCount = CountWords(entry.Content);

        _context.JournalEntries.Update(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task<bool> DeleteEntryAsync(int id)
    {
        var entry = await _context.JournalEntries.FindAsync(id);
        if (entry == null)
            return false;

        _context.JournalEntries.Remove(entry);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetTotalEntriesCountAsync()
    {
        return await _context.JournalEntries.CountAsync();
    }

    public async Task<Dictionary<DateTime, bool>> GetEntriesCalendarDataAsync(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var entries = await _context.JournalEntries
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .Select(e => e.Date.Date)
            .ToListAsync();

        var calendar = new Dictionary<DateTime, bool>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            calendar[date] = entries.Contains(date);
        }

        return calendar;
    }

    private int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

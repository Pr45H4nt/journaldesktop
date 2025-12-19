using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Models;

namespace JournalApp.Services;

public class TagService
{
    private readonly JournalDbContext _context;

    public TagService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.OrderBy(t => t.Name).ToListAsync();
    }

    public async Task<List<Tag>> GetPrebuiltTagsAsync()
    {
        return await _context.Tags
            .Where(t => !t.IsCustom)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<List<Tag>> GetCustomTagsAsync()
    {
        return await _context.Tags
            .Where(t => t.IsCustom)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Tag?> GetTagByNameAsync(string name)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<Tag> CreateCustomTagAsync(string name)
    {
        var existingTag = await GetTagByNameAsync(name);
        if (existingTag != null)
            return existingTag;

        var tag = new Tag
        {
            Name = name,
            IsCustom = true
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public async Task<bool> DeleteCustomTagAsync(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null || !tag.IsCustom)
            return false;

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Dictionary<string, int>> GetTagUsageStatsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.JournalEntries.Include(e => e.Tags).AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        var entries = await query.ToListAsync();
        var tagCounts = entries
            .SelectMany(e => e.Tags)
            .GroupBy(t => t.Name)
            .ToDictionary(g => g.Key, g => g.Count());

        return tagCounts.OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}

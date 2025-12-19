using Microsoft.EntityFrameworkCore;
using JournalApp.Models;

namespace JournalApp.Data;

public class JournalDbContext : DbContext
{
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<AppSettings> AppSettings { get; set; }

    public JournalDbContext(DbContextOptions<JournalDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JournalEntry>()
            .HasIndex(e => e.Date)
            .IsUnique();

        modelBuilder.Entity<JournalEntry>()
            .HasMany(e => e.Tags)
            .WithMany(t => t.JournalEntries)
            .UsingEntity(j => j.ToTable("JournalEntryTags"));

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        SeedPrebuiltTags(modelBuilder);
    }

    private void SeedPrebuiltTags(ModelBuilder modelBuilder)
    {
        var prebuiltTags = new[]
        {
            "Work", "Career", "Studies", "Family", "Friends", "Relationships",
            "Health", "Fitness", "Personal Growth", "Self-care", "Hobbies",
            "Travel", "Nature", "Finance", "Spirituality", "Birthday",
            "Holiday", "Vacation", "Celebration", "Exercise", "Reading",
            "Writing", "Cooking", "Meditation", "Yoga", "Music",
            "Shopping", "Parenting", "Projects", "Planning", "Reflection"
        };

        var tags = prebuiltTags.Select((tag, index) => new Tag
        {
            Id = index + 1,
            Name = tag,
            IsCustom = false
        }).ToArray();

        modelBuilder.Entity<Tag>().HasData(tags);
    }
}

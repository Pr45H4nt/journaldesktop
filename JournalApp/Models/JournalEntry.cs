using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models;

public class JournalEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public string? Title { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Required]
    public string PrimaryMood { get; set; } = string.Empty;

    public string? SecondaryMood1 { get; set; }

    public string? SecondaryMood2 { get; set; }

    public string? Category { get; set; }

    public int WordCount { get; set; }

    public List<Tag> Tags { get; set; } = new();
}

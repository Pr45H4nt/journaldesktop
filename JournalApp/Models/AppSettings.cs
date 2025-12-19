using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models;

public class AppSettings
{
    [Key]
    public int Id { get; set; }

    public string? PasswordHash { get; set; }

    public bool IsPasswordEnabled { get; set; }

    public bool IsDarkMode { get; set; }

    public DateTime? LastAccessDate { get; set; }

    public int CurrentStreak { get; set; }

    public int LongestStreak { get; set; }
}

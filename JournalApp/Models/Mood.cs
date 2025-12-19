namespace JournalApp.Models;

public enum MoodCategory
{
    Positive,
    Neutral,
    Negative
}

public class Mood
{
    public string Name { get; set; } = string.Empty;
    public MoodCategory Category { get; set; }
    public string Icon { get; set; } = string.Empty;

    public static readonly List<Mood> AllMoods = new()
    {
        // Positive Moods
        new Mood { Name = "Happy", Category = MoodCategory.Positive, Icon = "😊" },
        new Mood { Name = "Excited", Category = MoodCategory.Positive, Icon = "🤩" },
        new Mood { Name = "Relaxed", Category = MoodCategory.Positive, Icon = "😌" },
        new Mood { Name = "Grateful", Category = MoodCategory.Positive, Icon = "🙏" },
        new Mood { Name = "Confident", Category = MoodCategory.Positive, Icon = "💪" },

        // Neutral Moods
        new Mood { Name = "Calm", Category = MoodCategory.Neutral, Icon = "😐" },
        new Mood { Name = "Thoughtful", Category = MoodCategory.Neutral, Icon = "🤔" },
        new Mood { Name = "Curious", Category = MoodCategory.Neutral, Icon = "🧐" },
        new Mood { Name = "Nostalgic", Category = MoodCategory.Neutral, Icon = "😔" },
        new Mood { Name = "Bored", Category = MoodCategory.Neutral, Icon = "😑" },

        // Negative Moods
        new Mood { Name = "Sad", Category = MoodCategory.Negative, Icon = "😢" },
        new Mood { Name = "Angry", Category = MoodCategory.Negative, Icon = "😠" },
        new Mood { Name = "Stressed", Category = MoodCategory.Negative, Icon = "😰" },
        new Mood { Name = "Lonely", Category = MoodCategory.Negative, Icon = "😞" },
        new Mood { Name = "Anxious", Category = MoodCategory.Negative, Icon = "😟" }
    };
}

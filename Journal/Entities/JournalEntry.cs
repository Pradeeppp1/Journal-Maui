namespace Journal.Entities;

public class JournalEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string Mood { get; set; } = "Neutral";
    public string SecondaryMoods { get; set; } = string.Empty; // Comma-separated secondary moods
    public string Tags { get; set; } = string.Empty; // Comma-separated tags
    public int WordCount { get; set; }
    public int CharacterCount { get; set; }
    public int UserId { get; set; }
}

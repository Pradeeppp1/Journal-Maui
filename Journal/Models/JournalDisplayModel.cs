namespace Journal.Models;

public class JournalDisplayModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Mood { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public int WordCount { get; set; }
    public int CharacterCount { get; set; }
}

namespace Filmoteka.Models;

public sealed class Review
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Author { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime ReviewedAt { get; set; } = DateTime.Today;
}

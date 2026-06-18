namespace Filmoteka.Models;

public sealed class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
}

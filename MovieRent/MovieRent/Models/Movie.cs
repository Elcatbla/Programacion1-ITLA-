namespace MovieRent.Models;

public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public override string ToString()
    {
        string status = IsAvailable ? "Available" : "Rented";
        return $"[{MovieId}] {Title} ({ReleaseYear}) - {Genre} - {DurationMinutes} min - {status}";
    }
}
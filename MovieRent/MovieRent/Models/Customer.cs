namespace MovieRent.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string IdNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public override string ToString() => $"[{CustomerId}] {FullName} - ID: {IdNumber} - Phone: {Phone}";
}
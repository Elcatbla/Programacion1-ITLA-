namespace MovieRent.Models;

public class Rental
{
    public int RentalId { get; set; }
    public int MovieId { get; set; }
    public int CustomerId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }
    public DateTime CreatedAt { get; set; }

    public Movie? Movie { get; set; }
    public Customer? Customer { get; set; }

    public override string ToString()
    {
        string status = IsReturned
            ? $"Returned on {ReturnDate:dd/MM/yyyy}"
            : $"Pending (due: {DueDate:dd/MM/yyyy})";
        return $"[{RentalId}] MovieId: {MovieId} - CustomerId: {CustomerId} - " +
               $"Rented: {RentalDate:dd/MM/yyyy} - {status}";
    }
}

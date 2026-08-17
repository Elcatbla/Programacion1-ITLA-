using Microsoft.EntityFrameworkCore;
using MovieRent.Data;
using MovieRent.Models;

namespace MovieRent.Services;

public class RentalService(DataContext context)
{
    // ---------- MOVIES ----------

    public Movie AddMovie(string title, string genre, int releaseYear, int durationMinutes)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("The movie title cannot be empty.");
        if (releaseYear <= 1888 || releaseYear > DateTime.Now.Year + 1)
            throw new ArgumentException("The release year is not valid.");
        if (durationMinutes <= 0)
            throw new ArgumentException("The duration must be a number greater than zero.");

        var movie = new Movie
        {
            Title = title.Trim(),
            Genre = string.IsNullOrWhiteSpace(genre) ? "Unspecified" : genre.Trim(),
            ReleaseYear = releaseYear,
            DurationMinutes = durationMinutes,
            IsAvailable = true,
            CreatedAt = DateTime.Now
        };

        context.Movies.Add(movie);
        context.SaveChanges();
        return movie;
    }

    public List<Movie> GetAllMovies() =>
        context.Movies.OrderBy(m => m.MovieId).ToList();

    public List<Movie> GetAvailableMovies() =>
        context.Movies.Where(m => m.IsAvailable).OrderBy(m => m.MovieId).ToList();

    public List<Movie> GetRentedMovies() =>
        context.Movies.Where(m => !m.IsAvailable).OrderBy(m => m.MovieId).ToList();

    public Movie GetMovieById(int id)
    {
        var movie = context.Movies.FirstOrDefault(m => m.MovieId == id);
        if (movie is null)
            throw new ArgumentException($"There is no movie with Id {id}.");
        return movie;
    }

    public void UpdateMovie(int id, string title, string genre, int releaseYear, int durationMinutes)
    {
        var movie = GetMovieById(id);

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("The movie title cannot be empty.");
        if (releaseYear <= 1888 || releaseYear > DateTime.Now.Year + 1)
            throw new ArgumentException("The release year is not valid.");
        if (durationMinutes <= 0)
            throw new ArgumentException("The duration must be a number greater than zero.");

        movie.Title = title.Trim();
        movie.Genre = string.IsNullOrWhiteSpace(genre) ? "Unspecified" : genre.Trim();
        movie.ReleaseYear = releaseYear;
        movie.DurationMinutes = durationMinutes;
        movie.UpdatedAt = DateTime.Now;

        context.SaveChanges();
    }

    public void DeleteMovie(int id)
    {
        var movie = GetMovieById(id);

        if (!movie.IsAvailable)
            throw new InvalidOperationException("A movie that is currently rented cannot be deleted.");

        context.Movies.Remove(movie);
        context.SaveChanges();
    }

    // ---------- CUSTOMERS ----------

    public Customer AddCustomer(string fullName, string idNumber, string phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("The customer name cannot be empty.");
        if (string.IsNullOrWhiteSpace(idNumber))
            throw new ArgumentException("The customer ID number cannot be empty.");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("The customer phone cannot be empty.");
        if (context.Customers.Any(c => c.IdNumber == idNumber.Trim()))
            throw new ArgumentException("A customer with that ID number already exists.");

        var customer = new Customer
        {
            FullName = fullName.Trim(),
            IdNumber = idNumber.Trim(),
            Phone = phone.Trim(),
            CreatedAt = DateTime.Now
        };

        context.Customers.Add(customer);
        context.SaveChanges();
        return customer;
    }

    public List<Customer> GetAllCustomers() =>
        context.Customers.OrderBy(c => c.CustomerId).ToList();

    public Customer GetCustomerById(int id)
    {
        var customer = context.Customers.FirstOrDefault(c => c.CustomerId == id);
        if (customer is null)
            throw new ArgumentException($"There is no customer with Id {id}.");
        return customer;
    }

    public void UpdateCustomer(int id, string fullName, string idNumber, string phone)
    {
        var customer = GetCustomerById(id);

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("The customer name cannot be empty.");
        if (string.IsNullOrWhiteSpace(idNumber))
            throw new ArgumentException("The customer ID number cannot be empty.");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("The customer phone cannot be empty.");
        if (context.Customers.Any(c => c.IdNumber == idNumber.Trim() && c.CustomerId != id))
            throw new ArgumentException("Another customer already has that ID number.");

        customer.FullName = fullName.Trim();
        customer.IdNumber = idNumber.Trim();
        customer.Phone = phone.Trim();
        customer.UpdatedAt = DateTime.Now;

        context.SaveChanges();
    }

    public void DeleteCustomer(int id)
    {
        var customer = GetCustomerById(id);

        bool hasActiveRentals = context.Rentals.Any(r => r.CustomerId == id && !r.IsReturned);
        if (hasActiveRentals)
            throw new InvalidOperationException("A customer with pending rentals cannot be deleted.");

        context.Customers.Remove(customer);
        context.SaveChanges();
    }

    // ---------- RENTALS ----------

    public Rental CreateRental(int movieId, int customerId, DateTime rentalDate, DateTime dueDate)
    {
        var movie = GetMovieById(movieId);
        var customer = GetCustomerById(customerId);

        if (!movie.IsAvailable)
            throw new InvalidOperationException($"The movie '{movie.Title}' is not available for rental.");
        if (dueDate < rentalDate)
            throw new ArgumentException("The due date cannot be earlier than the rental date.");

        var rental = new Rental
        {
            MovieId = movie.MovieId,
            CustomerId = customer.CustomerId,
            RentalDate = rentalDate,
            DueDate = dueDate,
            IsReturned = false,
            CreatedAt = DateTime.Now
        };

        movie.IsAvailable = false;

        context.Rentals.Add(rental);
        context.SaveChanges();
        return rental;
    }

    public void ReturnRental(int rentalId)
    {
        var rental = context.Rentals.FirstOrDefault(r => r.RentalId == rentalId);
        if (rental is null)
            throw new ArgumentException($"There is no rental with Id {rentalId}.");
        if (rental.IsReturned)
            throw new InvalidOperationException("This rental was already returned.");

        var movie = GetMovieById(rental.MovieId);

        rental.IsReturned = true;
        rental.ReturnDate = DateTime.Now;
        movie.IsAvailable = true;

        context.SaveChanges();
    }

    public List<Rental> GetAllRentals() =>
        context.Rentals
            .Include(r => r.Movie)
            .Include(r => r.Customer)
            .OrderBy(r => r.RentalId)
            .ToList();

    public List<Rental> GetPendingRentals() =>
        context.Rentals
            .Include(r => r.Movie)
            .Include(r => r.Customer)
            .Where(r => !r.IsReturned)
            .OrderBy(r => r.RentalId)
            .ToList();
}
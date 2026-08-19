using Microsoft.EntityFrameworkCore;
using MovieRent.Data;
using MovieRent.Models;

namespace MovieRent.Services
{
    public class RentalService
    {
        private readonly DataContext _context;

        public RentalService(DataContext context)
        {
            _context = context;
        }

        // MOVIES

        public Movie AddMovie(string title, string genre, int releaseYear, int durationMinutes)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("The movie title cannot be empty.");
            }

            if (releaseYear <= 1888 || releaseYear > DateTime.Now.Year + 1)
            {
                throw new ArgumentException("The release year is not valid.");
            }

            if (durationMinutes <= 0)
            {
                throw new ArgumentException("The duration must be greater than zero.");
            }

            Movie movie = new Movie();

            movie.Title = title.Trim();

            if (string.IsNullOrWhiteSpace(genre))
            {
                movie.Genre = "Unspecified";
            }
            else
            {
                movie.Genre = genre.Trim();
            }

            movie.ReleaseYear = releaseYear;
            movie.DurationMinutes = durationMinutes;
            movie.IsAvailable = true;
            movie.CreatedAt = DateTime.Now;

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return movie;
        }

        public List<Movie> GetAllMovies()
        {
            return _context.Movies
                .OrderBy(m => m.MovieId)
                .ToList();
        }

        public List<Movie> GetAvailableMovies()
        {
            return _context.Movies
                .Where(m => m.IsAvailable == true)
                .OrderBy(m => m.MovieId)
                .ToList();
        }

        public List<Movie> GetRentedMovies()
        {
            return _context.Movies
                .Where(m => m.IsAvailable == false)
                .OrderBy(m => m.MovieId)
                .ToList();
        }

        public Movie GetMovieById(int id)
        {
            Movie movie = _context.Movies
                .FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
            {
                throw new ArgumentException("There is no movie with that ID.");
            }

            return movie;
        }

        public void UpdateMovie(int id, string title, string genre, int releaseYear, int durationMinutes)
        {
            Movie movie = GetMovieById(id);

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("The movie title cannot be empty.");
            }

            if (releaseYear <= 1888 || releaseYear > DateTime.Now.Year + 1)
            {
                throw new ArgumentException("The release year is not valid.");
            }

            if (durationMinutes <= 0)
            {
                throw new ArgumentException("The duration must be greater than zero.");
            }

            movie.Title = title.Trim();

            if (string.IsNullOrWhiteSpace(genre))
            {
                movie.Genre = "Unspecified";
            }
            else
            {
                movie.Genre = genre.Trim();
            }

            movie.ReleaseYear = releaseYear;
            movie.DurationMinutes = durationMinutes;
            movie.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteMovie(int id)
        {
            Movie movie = GetMovieById(id);

            if (movie.IsAvailable == false)
            {
                throw new InvalidOperationException(
                    "A movie that is currently rented cannot be deleted.");
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }


        // CUSTOMERS

        public Customer AddCustomer(string fullName, string idNumber, string phone)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("The customer name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                throw new ArgumentException("The customer ID number cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("The customer phone cannot be empty.");
            }

            Customer existingCustomer = _context.Customers
                .FirstOrDefault(c => c.IdNumber == idNumber.Trim());

            if (existingCustomer != null)
            {
                throw new ArgumentException(
                    "A customer with that ID number already exists.");
            }

            Customer customer = new Customer();

            customer.FullName = fullName.Trim();
            customer.IdNumber = idNumber.Trim();
            customer.Phone = phone.Trim();
            customer.CreatedAt = DateTime.Now;

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers
                .OrderBy(c => c.CustomerId)
                .ToList();
        }

        public Customer GetCustomerById(int id)
        {
            Customer customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
            {
                throw new ArgumentException(
                    "There is no customer with that ID.");
            }

            return customer;
        }

        public void UpdateCustomer(int id, string fullName, string idNumber, string phone)
        {
            Customer customer = GetCustomerById(id);

            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException(
                    "The customer name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                throw new ArgumentException(
                    "The customer ID number cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException(
                    "The customer phone cannot be empty.");
            }

            Customer otherCustomer = _context.Customers
                .FirstOrDefault(c =>
                    c.IdNumber == idNumber.Trim() &&
                    c.CustomerId != id);

            if (otherCustomer != null)
            {
                throw new ArgumentException(
                    "Another customer already has that ID number.");
            }

            customer.FullName = fullName.Trim();
            customer.IdNumber = idNumber.Trim();
            customer.Phone = phone.Trim();
            customer.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            Customer customer = GetCustomerById(id);

            bool hasActiveRentals = _context.Rentals
                .Any(r => r.CustomerId == id && r.IsReturned == false);

            if (hasActiveRentals)
            {
                throw new InvalidOperationException(
                    "A customer with pending rentals cannot be deleted.");
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }


        // RENTALS

        public Rental CreateRental(
            int movieId,
            int customerId,
            DateTime rentalDate,
            DateTime dueDate)
        {
            Movie movie = GetMovieById(movieId);
            Customer customer = GetCustomerById(customerId);

            if (movie.IsAvailable == false)
            {
                throw new InvalidOperationException(
                    "The movie is not available for rental.");
            }

            if (dueDate < rentalDate)
            {
                throw new ArgumentException(
                    "The due date cannot be earlier than the rental date.");
            }

            Rental rental = new Rental();

            rental.MovieId = movie.MovieId;
            rental.CustomerId = customer.CustomerId;
            rental.RentalDate = rentalDate;
            rental.DueDate = dueDate;
            rental.IsReturned = false;
            rental.CreatedAt = DateTime.Now;

            movie.IsAvailable = false;

            _context.Rentals.Add(rental);
            _context.SaveChanges();

            return rental;
        }

        public void ReturnRental(int rentalId)
        {
            Rental rental = _context.Rentals
                .FirstOrDefault(r => r.RentalId == rentalId);

            if (rental == null)
            {
                throw new ArgumentException(
                    "There is no rental with that ID.");
            }

            if (rental.IsReturned == true)
            {
                throw new InvalidOperationException(
                    "This rental was already returned.");
            }

            Movie movie = GetMovieById(rental.MovieId);

            rental.IsReturned = true;
            rental.ReturnDate = DateTime.Now;

            movie.IsAvailable = true;

            _context.SaveChanges();
        }

        public List<Rental> GetAllRentals()
        {
            return _context.Rentals
                .Include(r => r.Movie)
                .Include(r => r.Customer)
                .OrderBy(r => r.RentalId)
                .ToList();
        }

        public List<Rental> GetPendingRentals()
        {
            return _context.Rentals
                .Include(r => r.Movie)
                .Include(r => r.Customer)
                .Where(r => r.IsReturned == false)
                .OrderBy(r => r.RentalId)
                .ToList();
        }
    }
}
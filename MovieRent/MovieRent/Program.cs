using System.Globalization;
using MovieRent.Data;
using MovieRent.Models;
using MovieRent.Services;

DataContext context = new DataContext();
RentalService service = new RentalService(context);

bool exit = false;

while (exit == false)
{
    PrintHeader("MAIN MENU");

    Console.WriteLine("1. Movie Management");
    Console.WriteLine("2. Customer Management");
    Console.WriteLine("3. Rental Management");
    Console.WriteLine("4. Reports");
    Console.WriteLine("0. Exit");

    Console.Write("\nSelect an option: ");

    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            MovieMenu();
            break;

        case "2":
            CustomerMenu();
            break;

        case "3":
            RentalMenu();
            break;

        case "4":
            ReportsMenu();
            break;

        case "0":
            exit = true;
            break;

        default:
            PrintError("Invalid option.");
            break;
    }
}

Console.WriteLine("\nGoodbye.");


// ================= MOVIE MENU =================

void MovieMenu()
{
    bool back = false;

    while (back == false)
    {
        PrintHeader("MOVIE MANAGEMENT");

        Console.WriteLine("1. Register movie");
        Console.WriteLine("2. List movies");
        Console.WriteLine("3. Update movie");
        Console.WriteLine("4. Delete movie");
        Console.WriteLine("0. Back");

        Console.Write("\nSelect an option: ");

        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                RegisterMovie();
                break;

            case "2":
                PrintHeader("MOVIES");
                PrintMovies(service.GetAllMovies());
                Pause();
                break;

            case "3":
                UpdateMovie();
                break;

            case "4":
                DeleteMovie();
                break;

            case "0":
                back = true;
                break;

            default:
                PrintError("Invalid option.");
                break;
        }
    }
}


void RegisterMovie()
{
    PrintHeader("REGISTER MOVIE");

    try
    {
        string title = ReadText("Title: ");
        string genre = ReadText("Genre: ", true);
        int releaseYear = ReadInt("Release year: ");
        int duration = ReadInt("Duration (minutes): ");

        Movie movie = service.AddMovie(
            title,
            genre,
            releaseYear,
            duration);

        PrintSuccess("Movie registered with Id " + movie.MovieId);
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


void UpdateMovie()
{
    PrintHeader("UPDATE MOVIE");

    try
    {
        PrintMovies(service.GetAllMovies());

        int id = ReadInt("\nMovie Id to update: ");

        string title = ReadText("New title: ");
        string genre = ReadText("New genre: ", true);
        int releaseYear = ReadInt("New release year: ");
        int duration = ReadInt("New duration (minutes): ");

        service.UpdateMovie(
            id,
            title,
            genre,
            releaseYear,
            duration);

        PrintSuccess("Movie updated successfully.");
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


void DeleteMovie()
{
    PrintHeader("DELETE MOVIE");

    try
    {
        PrintMovies(service.GetAllMovies());

        int id = ReadInt("\nMovie Id to delete: ");

        service.DeleteMovie(id);

        PrintSuccess("Movie deleted successfully.");
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


// ================= CUSTOMER MENU =================

void CustomerMenu()
{
    bool back = false;

    while (back == false)
    {
        PrintHeader("CUSTOMER MANAGEMENT");

        Console.WriteLine("1. Register customer");
        Console.WriteLine("2. List customers");
        Console.WriteLine("3. Update customer");
        Console.WriteLine("4. Delete customer");
        Console.WriteLine("0. Back");

        Console.Write("\nSelect an option: ");

        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                RegisterCustomer();
                break;

            case "2":
                PrintHeader("CUSTOMERS");
                PrintCustomers(service.GetAllCustomers());
                Pause();
                break;

            case "3":
                UpdateCustomer();
                break;

            case "4":
                DeleteCustomer();
                break;

            case "0":
                back = true;
                break;

            default:
                PrintError("Invalid option.");
                break;
        }
    }
}


void RegisterCustomer()
{
    PrintHeader("REGISTER CUSTOMER");

    try
    {
        string fullName = ReadText("Full name: ");
        string idNumber = ReadText("ID number: ");
        string phone = ReadText("Phone: ");

        Customer customer = service.AddCustomer(
            fullName,
            idNumber,
            phone);

        PrintSuccess(
            "Customer registered with Id " +
            customer.CustomerId);
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


void UpdateCustomer()
{
    PrintHeader("UPDATE CUSTOMER");

    try
    {
        PrintCustomers(service.GetAllCustomers());

        int id = ReadInt("\nCustomer Id to update: ");

        string fullName = ReadText("New full name: ");
        string idNumber = ReadText("New ID number: ");
        string phone = ReadText("New phone: ");

        service.UpdateCustomer(
            id,
            fullName,
            idNumber,
            phone);

        PrintSuccess("Customer updated successfully.");
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


void DeleteCustomer()
{
    PrintHeader("DELETE CUSTOMER");

    try
    {
        PrintCustomers(service.GetAllCustomers());

        int id = ReadInt("\nCustomer Id to delete: ");

        service.DeleteCustomer(id);

        PrintSuccess("Customer deleted successfully.");
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


// ================= RENTAL MENU =================

void RentalMenu()
{
    bool back = false;

    while (back == false)
    {
        PrintHeader("RENTAL MANAGEMENT");

        Console.WriteLine("1. Create rental");
        Console.WriteLine("2. Register return");
        Console.WriteLine("3. List rentals");
        Console.WriteLine("0. Back");

        Console.Write("\nSelect an option: ");

        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                CreateRental();
                break;

            case "2":
                RegisterReturn();
                break;

            case "3":
                PrintHeader("RENTALS");
                PrintRentals(service.GetAllRentals());
                Pause();
                break;

            case "0":
                back = true;
                break;

            default:
                PrintError("Invalid option.");
                break;
        }
    }
}


void CreateRental()
{
    PrintHeader("CREATE RENTAL");

    try
    {
        Console.WriteLine("Available movies:");

        List<Movie> movies = service.GetAvailableMovies();
        PrintMovies(movies);

        int movieId = ReadInt("\nMovie Id: ");

        Console.WriteLine();

        List<Customer> customers = service.GetAllCustomers();
        PrintCustomers(customers);

        int customerId = ReadInt("\nCustomer Id: ");

        DateTime rentalDate = ReadDate(
            "Rental date (dd/MM/yyyy) [Enter = today]: ",
            DateTime.Now);

        DateTime dueDate = ReadDate(
            "Due date (dd/MM/yyyy): ",
            null);

        Rental rental = service.CreateRental(
            movieId,
            customerId,
            rentalDate,
            dueDate);

        PrintSuccess(
            "Rental registered with Id " +
            rental.RentalId);
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


void RegisterReturn()
{
    PrintHeader("REGISTER RETURN");

    try
    {
        Console.WriteLine("Pending rentals:");

        List<Rental> rentals = service.GetPendingRentals();
        PrintRentals(rentals);

        int rentalId = ReadInt("\nRental Id to return: ");

        service.ReturnRental(rentalId);

        PrintSuccess("Return registered successfully.");
    }
    catch (Exception ex)
    {
        PrintError(ex.Message);
    }

    Pause();
}


// ================= REPORTS MENU =================

void ReportsMenu()
{
    bool back = false;

    while (back == false)
    {
        PrintHeader("REPORTS");

        Console.WriteLine("1. All movies");
        Console.WriteLine("2. Available movies");
        Console.WriteLine("3. Rented movies");
        Console.WriteLine("4. All customers");
        Console.WriteLine("5. All rentals");
        Console.WriteLine("0. Back");

        Console.Write("\nSelect an option: ");

        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                PrintHeader("ALL MOVIES");
                PrintMovies(service.GetAllMovies());
                Pause();
                break;

            case "2":
                PrintHeader("AVAILABLE MOVIES");
                PrintMovies(service.GetAvailableMovies());
                Pause();
                break;

            case "3":
                PrintHeader("RENTED MOVIES");
                PrintMovies(service.GetRentedMovies());
                Pause();
                break;

            case "4":
                PrintHeader("ALL CUSTOMERS");
                PrintCustomers(service.GetAllCustomers());
                Pause();
                break;

            case "5":
                PrintHeader("ALL RENTALS");
                PrintRentals(service.GetAllRentals());
                Pause();
                break;

            case "0":
                back = true;
                break;

            default:
                PrintError("Invalid option.");
                break;
        }
    }
}


// ================= LIST HELPERS =================

void PrintMovies(List<Movie> movies)
{
    if (movies.Count == 0)
    {
        Console.WriteLine("(No movies to show)");
        return;
    }

    foreach (Movie movie in movies)
    {
        Console.WriteLine(movie);
    }
}


void PrintCustomers(List<Customer> customers)
{
    if (customers.Count == 0)
    {
        Console.WriteLine("(No customers to show)");
        return;
    }

    foreach (Customer customer in customers)
    {
        Console.WriteLine(customer);
    }
}


void PrintRentals(List<Rental> rentals)
{
    if (rentals.Count == 0)
    {
        Console.WriteLine("(No rentals to show)");
        return;
    }

    foreach (Rental rental in rentals)
    {
        string movieTitle;

        if (rental.Movie != null)
        {
            movieTitle = rental.Movie.Title;
        }
        else
        {
            movieTitle = "(unknown)";
        }

        string customerName;

        if (rental.Customer != null)
        {
            customerName = rental.Customer.FullName;
        }
        else
        {
            customerName = "(unknown)";
        }

        Console.WriteLine(
            rental +
            " | Movie: " + movieTitle +
            " | Customer: " + customerName);
    }
}


// ================= INPUT / OUTPUT HELPERS =================

void PrintHeader(string title)
{
    Console.Clear();

    Console.WriteLine(new string('=', 50));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 50));
}


void PrintError(string message)
{
    Console.WriteLine("\n[ERROR] " + message);
}


void PrintSuccess(string message)
{
    Console.WriteLine("\n[OK] " + message);
}


void Pause()
{
    Console.WriteLine("\nPress ENTER to continue...");
    Console.ReadLine();
}


string ReadText(string prompt, bool allowEmpty = false)
{
    while (true)
    {
        Console.Write(prompt);

        string value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        if (allowEmpty == true)
        {
            return "";
        }

        Console.WriteLine(
            "This field cannot be empty. Please try again.");
    }
}


int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);

        string input = Console.ReadLine();

        int value;

        if (int.TryParse(input, out value))
        {
            return value;
        }

        Console.WriteLine(
            "You must enter a valid integer. Please try again.");
    }
}


DateTime ReadDate(string prompt, DateTime? defaultValue)
{
    while (true)
    {
        Console.Write(prompt);

        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) &&
            defaultValue.HasValue)
        {
            return defaultValue.Value;
        }

        DateTime date;

        bool validDate = DateTime.TryParseExact(
            input,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out date);

        if (validDate)
        {
            return date;
        }

        Console.WriteLine(
            "Invalid date format. Use dd/MM/yyyy.");
    }
}
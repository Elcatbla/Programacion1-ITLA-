using MovieRent.Data;

using var context = new DataContext();

try
{
    Console.WriteLine("Testing connection to MovieRentalDB...\n");

    var movies = context.Movies.ToList();

    Console.WriteLine($"Connection successful. Found {movies.Count} movie(s):\n");
    foreach (var movie in movies)
    {
        Console.WriteLine(movie);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Connection FAILED.");
    Console.WriteLine($"Error: {ex.Message}");
}
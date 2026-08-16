using Microsoft.EntityFrameworkCore;
using MovieRent.Models;

namespace MovieRent.Data;

public class DataContext : DbContext
{
    // TODO: adjust the server name / authentication here if your setup is different.
    private const string ConnectionString =
        "Server=localhost\\SQLEXPRESS;Database=MovieRentalDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Rental> Rentals => Set<Rental>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies");
            entity.HasKey(m => m.MovieId);
            entity.Property(m => m.Title).HasMaxLength(150).IsRequired();
            entity.Property(m => m.Genre).HasMaxLength(50).IsRequired();
            entity.Property(m => m.ReleaseYear).IsRequired();
            entity.Property(m => m.DurationMinutes).IsRequired();
            entity.Property(m => m.IsAvailable).IsRequired();
            entity.Property(m => m.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(c => c.CustomerId);
            entity.Property(c => c.FullName).HasMaxLength(120).IsRequired();
            entity.Property(c => c.IdNumber).HasMaxLength(20).IsRequired();
            entity.HasIndex(c => c.IdNumber).IsUnique();
            entity.Property(c => c.Phone).HasMaxLength(20).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.ToTable("Rentals");
            entity.HasKey(r => r.RentalId);
            entity.Property(r => r.RentalDate).IsRequired();
            entity.Property(r => r.DueDate).IsRequired();
            entity.Property(r => r.IsReturned).IsRequired();
            entity.Property(r => r.CreatedAt).IsRequired();

            entity.HasOne(r => r.Movie)
                  .WithMany(m => m.Rentals)
                  .HasForeignKey(r => r.MovieId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Customer)
                  .WithMany(c => c.Rentals)
                  .HasForeignKey(r => r.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
using Microsoft.EntityFrameworkCore;
using MoviesLibraryApp.Data.Entities;

namespace MoviesLibraryApp.Data;
public class MoviesLibraryAppDbContext : DbContext
{
    public MoviesLibraryAppDbContext(DbContextOptions<MoviesLibraryAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Series> Series { get; set; }
}
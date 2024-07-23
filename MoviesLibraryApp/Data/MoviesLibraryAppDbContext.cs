using Microsoft.EntityFrameworkCore;
using MoviesLibraryApp.Entities;

namespace MoviesLibraryApp.Data;
public class MoviesLibraryAppDbContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Series> Series => Set<Series>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase("StorageAppDb");
    }
}

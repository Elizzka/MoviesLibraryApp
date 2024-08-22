using MoviesLibraryApp.Data.Entities;
using MoviesLibraryApp.Data.Repositories;

namespace MoviesLibraryApp.Components.DataProviders;

public class MovieProvider : IMovieProvider
{
    private readonly IRepository<Movie> _movieRepo;
    public MovieProvider(IRepository<Movie> repository)
    {
        _movieRepo = repository;
    }

    public Movie? FirstOrDefaultByMovieType(string type)
    {
        var movies = _movieRepo.GetAll();
        return movies.FirstOrDefault(x => x.Type == type);
    }

    public List<string> GetUniqueMovieType()
    {
        var movies = _movieRepo.GetAll();
        var types = movies.Select(x => x.Type).Distinct().ToList();
        return types;
    }

    public List<Movie> OrderByMovieType()
    {
        var movies = _movieRepo.GetAll();
        return movies.OrderBy(x => x.Type).ToList();
    }

    public Movie? SingleOrDefaultByMovieId(int id)
    {
        var movies = _movieRepo.GetAll();
        return movies.SingleOrDefault(x => x.Id == id);
    }
}

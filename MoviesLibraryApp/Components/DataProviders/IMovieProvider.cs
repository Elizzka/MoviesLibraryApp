using MoviesLibraryApp.Data.Entities;

namespace MoviesLibraryApp.Components.DataProviders;

public interface IMovieProvider
{
    List<string> GetUniqueMovieType();
    List<Movie> OrderByMovieType();
    Movie? FirstOrDefaultByMovieType(string type);
    Movie? SingleOrDefaultByMovieId(int id);
}

using MoviesLibraryApp.Entities;

namespace MoviesLibraryApp.DataProviders;

public interface ISeriesProvider
{
    List<string> GetUniqueSeriesType();
    List<Series> OrderBySeriesType();
    Series? FirstOrDefaultBySeriesType(string type);
    Series? SingleOrDefaultBySeriesId(int id);
}

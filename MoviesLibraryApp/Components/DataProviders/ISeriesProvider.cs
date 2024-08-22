using MoviesLibraryApp.Data.Entities;

namespace MoviesLibraryApp.Components.DataProviders;

public interface ISeriesProvider
{
    List<string> GetUniqueSeriesType();
    List<Series> OrderBySeriesType();
    Series? FirstOrDefaultBySeriesType(string type);
    Series? SingleOrDefaultBySeriesId(int id);
}

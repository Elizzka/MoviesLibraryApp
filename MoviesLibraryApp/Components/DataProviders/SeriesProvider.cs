using MoviesLibraryApp.Data.Entities;
using MoviesLibraryApp.Data.Repositories;

namespace MoviesLibraryApp.Components.DataProviders;

public class SeriesProvider : ISeriesProvider
{
    private readonly IRepository<Series> _seriesRepo;
    public SeriesProvider(IRepository<Series> repository)
    {
        _seriesRepo = repository;
    }

    public Series? FirstOrDefaultBySeriesType(string type)
    {
        var series = _seriesRepo.GetAll();
        return series.FirstOrDefault(x => x.Type == type);
    }

    public List<string> GetUniqueSeriesType()
    {
        var series = _seriesRepo.GetAll();
        var types = series.Select(x => x.Type).Distinct().ToList();
        return types;
    }

    public List<Series> OrderBySeriesType()
    {
        var series = _seriesRepo.GetAll();
        return series.OrderBy(x => x.Type).ToList();
    }

    public Series? SingleOrDefaultBySeriesId(int id)
    {
        var series = _seriesRepo.GetAll();
        return series.SingleOrDefault(x => x.Id == id);
    }
}

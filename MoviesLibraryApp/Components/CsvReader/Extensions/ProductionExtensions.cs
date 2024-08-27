using MoviesLibraryApp.Components.CsvReader.Models;

namespace MoviesLibraryApp.Components.CsvReader.Extensions;

public static class ProductionExtensions
{
    public static IEnumerable<Production> ToProduction(this IEnumerable<string> source)
    {
        foreach (var line in source)
        {
            var columns = line.Split(';');

            yield return new Production
            {
                Title = columns[0],
                Year = int.Parse(columns[1]),
                Director = columns[2],
                Type = columns[3],
                MovieOrSeries = columns[4]
            };
        }
    }
}

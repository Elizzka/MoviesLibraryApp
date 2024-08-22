using MoviesLibraryApp.Components.CsvReader.Models;

namespace MoviesLibraryApp.Components.CsvReader.Extensions;

public static class DirectorExtensions
{
    public static IEnumerable<Director> ToDirector(this IEnumerable<string> source)
    {
        foreach (var line in source)
        {
            var columns = line.Split(';');

            yield return new Director
            {
                Name = columns[0],
                YearOfBirth = int.Parse(columns[1]),
                CountryOfBirth = columns[2],
            };
        }
    }
}

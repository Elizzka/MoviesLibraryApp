using MoviesLibraryApp.Components.CsvReader.Extensions;
using MoviesLibraryApp.Components.CsvReader.Models;

namespace MoviesLibraryApp.Components.CsvReader;

public class CsvReader : ICsvReader
{
    public List<Director> ProcessDirectors(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Director>();
        }

        var directors = File.ReadAllLines(filePath)
            .Skip(1)
            .Where(x => x.Length > 1)
            .ToDirector();

        return directors.ToList();
    }

    public List<Production> ProcessProductions(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Production>();
        }

        var productions = File.ReadAllLines(filePath)
            .Skip(1)
            .Where(x => x.Length > 1)
            .ToProduction();

        return productions.ToList();
    }
}

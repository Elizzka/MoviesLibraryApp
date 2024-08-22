using MoviesLibraryApp.Components.CsvReader.Models;

namespace MoviesLibraryApp.Components.CsvReader;

public interface ICsvReader
{
    List<Director> ProcessDirectors(string filePath);
    List<Production> ProcessProductions(string filePath);

}

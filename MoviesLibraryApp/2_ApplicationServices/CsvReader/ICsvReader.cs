using MoviesLibraryApp.Models;

namespace MoviesLibraryApp.CsvReader
{
    public interface ICsvReader
    {
        List<Director> ProcessDirectors(string filePath);
        List<Production> ProcessProductions(string filePath);

    }
}

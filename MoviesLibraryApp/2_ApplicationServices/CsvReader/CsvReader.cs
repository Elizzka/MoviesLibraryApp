using MoviesLibraryApp.Extensions;
using MoviesLibraryApp.Models;

namespace MoviesLibraryApp.CsvReader
{
    public class CsvReader : ICsvReader
    {
        public List<T> ProcessData<T>(string filePath, Func<IEnumerable<string>, IEnumerable<T>> mapFunction)
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            var data = File.ReadAllLines(filePath)
                .Skip(1)
                .Where(x => x.Length > 1);

            return mapFunction(data).ToList();
        }

        public List<Director> ProcessDirectors(string filePath)
        {
            return ProcessData(filePath, data => data.ToDirector());
        }

        public List<Production> ProcessProductions(string filePath)
        {
            return ProcessData(filePath, data => data.ToProduction());
        }
    }
}

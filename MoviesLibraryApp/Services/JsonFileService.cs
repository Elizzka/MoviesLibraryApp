using System.Text.Json;

namespace MoviesLibraryApp.Services
{
    public class JsonFileService<T> : IJsonFileService<T>
    {
        private readonly string _filePath;

        public JsonFileService(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
            }
            else
            {
                return new List<T>();
            }
        }

        public void SaveToFile(IEnumerable<T> data)
        {
            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}



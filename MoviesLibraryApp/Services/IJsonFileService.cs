namespace MoviesLibraryApp.Services;

public interface IJsonFileService<T>
{
    List<T> LoadFromFile();
    void SaveToFile(IEnumerable<T> data);
}


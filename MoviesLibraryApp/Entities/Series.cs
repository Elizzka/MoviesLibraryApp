namespace MoviesLibraryApp.Entities;

public class Series : Movie
{
    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Type: {Type} (Series)";
    }

}

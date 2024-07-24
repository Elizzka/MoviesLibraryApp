namespace MoviesLibraryApp.Entities;

public class Series : Movie
{
    public override string ToString() => $"Id: {Id}, Title: {Title}, Type: {Type} (Series)";
}

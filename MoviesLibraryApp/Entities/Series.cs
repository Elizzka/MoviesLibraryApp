namespace MoviesLibraryApp.Entities;

public class Series : Movie
{
    public string Tittle { get; set; }
    public string Type { get; set; }
    public override string ToString() => $"Id: {Id}, Tittle: {Tittle}, Type: {Type} (Series)";
}

namespace MoviesLibraryApp.Entities;

public class EntityBase : IEntity
{
    public string Title { get; set; }
    public string Type { get; set; }
    public int Id { get; set; }
    public override string ToString() => $"Id: {Id}, Title: {Title}, Type: {Type}";
}
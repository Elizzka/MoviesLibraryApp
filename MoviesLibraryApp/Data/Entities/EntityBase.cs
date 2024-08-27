namespace MoviesLibraryApp.Data.Entities;

public class EntityBase : IEntity
{
    public string Title { get; set; }
    public int Year { get; set; }
    public string Director { get; set; }
    public string Type { get; set; }
    public int Id { get; set; }
    public override string ToString() => $"Id: {Id}, Title: {Title}, Year: {Year}, Director: {Director}, Type: {Type}";
}
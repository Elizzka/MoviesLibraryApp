namespace MoviesLibraryApp.Entities
{
    public class Movie : EntityBase
    {
        public string? Title { get; set; }
        public string? Type { get; set; }

        public override string ToString() => $"Id: {Id}, Title: {Title}, Type: {Type}";
    }
}

namespace MoviesLibraryApp.Entities
{
    public class Movie : EntityBase
    {
        public string Tittle { get; set; }

        public override string ToString() => $"Id: {Id}, Tittle: {Tittle}";
    }
}

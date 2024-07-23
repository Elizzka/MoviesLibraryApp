using MoviesLibraryApp.Repositories;
using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Data;

var movieRepository = new SqlRepository<Movie>(new MoviesLibraryAppDbContext());
AddMovie(movieRepository);
AddSeries(movieRepository);
WriteAllToConsole(movieRepository);

static void AddMovie(IRepository<Movie> movieRepository)
{
    movieRepository.Add(new Movie { Tittle = "Zielona mila" });
    movieRepository.Add(new Movie { Tittle = "Forrest Gump" });
    movieRepository.Add(new Movie { Tittle = "Joker" });
    movieRepository.Save();
}

static void AddSeries(IWriteRepository<Series> seriesRepository)
{
    seriesRepository.Add(new Series { Tittle = "Breaking Bad" });
    seriesRepository.Add(new Series { Tittle = "The Crown" });
    seriesRepository.Save();
}

static void WriteAllToConsole(IReadRepository<IEntity> repository)
{
    var items = repository.GetAll();
    foreach(var item in items)
    {
        Console.WriteLine(item);
    }
}


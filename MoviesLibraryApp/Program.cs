using MoviesLibraryApp.Repositories;
using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Data;

var movieRepository = new SqlRepository<Movie>(new MoviesLibraryAppDbContext());
AddMovie(movieRepository);
AddSeries(movieRepository);
WriteAllToConsole(movieRepository);

static void AddMovie(IRepository<Movie> movieRepository)
{
    movieRepository.Add(new Movie { Title = "The Green Mile", Type = "Drama" });
    movieRepository.Add(new Movie { Title = "Forrest Gump", Type = "Drama" });
    movieRepository.Add(new Movie { Title = "Joker", Type = "Thriller" });
    movieRepository.Save();
}

static void AddSeries(IWriteRepository<Series> seriesRepository)
{
    seriesRepository.Add(new Series { Title = "Breaking Bad", Type = "Crime" });
    seriesRepository.Add(new Series { Title = "The Crown", Type = "Historical" });
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


using MoviesLibraryApp.Repositories;
using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Data;

var movieRepository = new SqlRepository<Movie>(new MoviesLibraryAppDbContext());
AddMovie(movieRepository);
AddSeries(movieRepository);
WriteAllToConsole(movieRepository);

static void AddMovie(IRepository<Movie> movieRepository)
{
    movieRepository.Add(new Movie { Tittle = "ggftt" });
    movieRepository.Add(new Movie { Tittle = "kwis" });
    movieRepository.Add(new Movie { Tittle = "chdy" });
    movieRepository.Save();
}

static void AddSeries(IWriteRepository<Series> seriesRepository)
{
    seriesRepository.Add(new Series { Tittle = "ggftt" });
    seriesRepository.Add(new Series { Tittle = "kwis" });
    seriesRepository.Add(new Series { Tittle = "chdy" });
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


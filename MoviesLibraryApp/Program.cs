using MoviesLibraryApp.Repositories;
using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Data;
using MoviesLibraryApp.Services;

Console.WriteLine("Welcome to the movie library!");
Console.WriteLine("Select an option that interests you:");
Console.WriteLine("Enter '1' if you want to display the entire contents of the library.");
Console.WriteLine("Enter '2' if you want to remove an item from the library.");
Console.WriteLine("Enter '3' if you want to add a movie to the library.");
Console.WriteLine("Enter '4' if you want to add a series to the library.");
Console.WriteLine("Enter 'q' if you want to quit.");

var dbContext = new MoviesLibraryAppDbContext();
var movieRepository = new SqlRepository<Movie>(dbContext);
var seriesRepository = new SqlRepository<Series>(dbContext);

var jsonMovieService = new JsonFileService<Movie>("movies.json");
var jsonSeriesService = new JsonFileService<Series>("series.json");
var auditMovieService = new AuditService<Movie>("audit_log_movies.txt");
var auditSeriesService = new AuditService<Series>("audit_log_series.txt");

var moviesFromFile = jsonMovieService.LoadFromFile();
var seriesFromFile = jsonSeriesService.LoadFromFile();

if (moviesFromFile.Any())
{
    foreach (var movie in moviesFromFile)
    {
        movieRepository.Add(movie);
    }
}

if (seriesFromFile.Any())
{
    foreach (var series in seriesFromFile)
    {
        seriesRepository.Add(series);
    }
}

if (!movieRepository.GetAll().Any() && !seriesRepository.GetAll().Any())
{
    AddInitialMoviesAndSeries(movieRepository, seriesRepository, auditMovieService, auditSeriesService);
}

movieRepository.ItemAdded += (sender, movie) =>
{
    auditMovieService.LogAudit("Added Movie", movie);
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Added Movie - {movie.Title}");
};

seriesRepository.ItemAdded += (sender, series) =>
{
    auditSeriesService.LogAudit("Added Series", series);
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Added Series - {series.Title}");
};

movieRepository.ItemRemoved += (sender, movie) =>
{
    auditMovieService.LogAudit("Removed Movie", movie);
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Removed Movie - {movie.Title}");
};

seriesRepository.ItemRemoved += (sender, series) =>
{
    auditSeriesService.LogAudit("Removed Series", series);
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Removed Series - {series.Title}");
};

while (true)
{
    Console.WriteLine("Select another option:");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            WriteAllToConsole(movieRepository, seriesRepository);
            break;
        case "2":
            RemoveItemFromLibrary(movieRepository, seriesRepository);
            break;
        case "3":
            AddMovie(movieRepository);
            break;
        case "4":
            AddSeries(seriesRepository);
            break;
        case "q":
            jsonMovieService.SaveToFile(movieRepository.GetAll());
            jsonSeriesService.SaveToFile(seriesRepository.GetAll());
            return;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}

static void AddInitialMoviesAndSeries(IRepository<Movie> movieRepo, IRepository<Series> seriesRepo, AuditService<Movie> movieAuditService, AuditService<Series> seriesAuditService)
{
    var movies = new List<Movie>
    {
        new Movie { Title = "The Green Mile", Type = "Drama" },
        new Movie { Title = "Forrest Gump", Type = "Drama" },
        new Movie { Title = "Joker", Type = "Thriller" }
    };

    var series = new List<Series>
    {
        new Series { Title = "Breaking Bad", Type = "Crime" },
        new Series { Title = "The Crown", Type = "Historical" }
    };

    foreach (var movie in movies)
    {
        movieRepo.Add(movie);
        movieAuditService.LogAudit("Initial Add Movie", movie);
    }

    foreach (var serie in series)
    {
        seriesRepo.Add(serie);
        seriesAuditService.LogAudit("Initial Add Series", serie);
    }
}

static void WriteAllToConsole(IReadRepository<Movie> movieRepo, IReadRepository<Series> seriesRepo)
{
    var movies = movieRepo.GetAll();
    var series = seriesRepo.GetAll();

    Console.WriteLine("Movies:");
    foreach (var movie in movies)
    {
        Console.WriteLine(movie);
    }

    Console.WriteLine("\nSeries:");
    foreach (var serie in series)
    {
        Console.WriteLine(serie);
    }
}

static void RemoveItemFromLibrary(IRepository<Movie> movieRepo, IRepository<Series> seriesRepo)
{
    Console.WriteLine("Enter a title for the item you want to delete:");
    var title = Console.ReadLine();

    var movie = movieRepo.GetAll().FirstOrDefault(x => x.Title == title);
    if (movie != null)
    {
        movieRepo.Remove(movie);
        return;
    }

    var series = seriesRepo.GetAll().FirstOrDefault(x => x.Title == title);
    if (series != null)
    {
        seriesRepo.Remove(series);
    }
    else
    {
        Console.WriteLine($"No item with title '{title}' was found.");
    }
}

static void AddMovie(IRepository<Movie> movieRepo)
{
    Console.WriteLine("Enter the title of the movie you want to add:");
    var title = Console.ReadLine();

    Console.WriteLine("Please specify the type of the movie:");
    var type = Console.ReadLine();

    var movie = new Movie { Title = title, Type = type };
    movieRepo.Add(movie);
}

static void AddSeries(IRepository<Series> seriesRepo)
{
    Console.WriteLine("Enter the title of the series you want to add:");
    var title = Console.ReadLine();

    Console.WriteLine("Please specify the type of the series:");
    var type = Console.ReadLine();

    var series = new Series { Title = title, Type = type };
    seriesRepo.Add(series);
}

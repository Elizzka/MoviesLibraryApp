using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Repositories;
using MoviesLibraryApp.Services;

namespace MoviesLibraryApp.UserCommunication;

public class UserCommunication : IUserCommunication
{
    private readonly IRepository<Series> _seriesRepo;
    private readonly IRepository<Movie> _movieRepo;
    private readonly IJsonFileService<Movie> _jsonMovieService;
    private readonly IJsonFileService<Series> _jsonSeriesService;
    private readonly IAuditService<Movie> _movieAuditService;
    private readonly IAuditService<Series> _seriesAuditService;

    public UserCommunication(
        IRepository<Series> seriesRepo,
        IRepository<Movie> movieRepo,
        IJsonFileService<Movie> jsonMovieService,
        IJsonFileService<Series> jsonSeriesService,
        IAuditService<Movie> movieAuditService,
        IAuditService<Series> seriesAuditService)
    {
        _seriesRepo = seriesRepo;
        _movieRepo = movieRepo;
        _jsonMovieService = jsonMovieService;
        _jsonSeriesService = jsonSeriesService;
        _movieAuditService = movieAuditService;
        _seriesAuditService = seriesAuditService;

        _movieRepo.ItemAdded += (sender, movie) =>
        {
            _movieAuditService.LogAudit("Added Movie", movie);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Added Movie - {movie.Title}");
        };

        _seriesRepo.ItemAdded += (sender, series) =>
        {
            _seriesAuditService.LogAudit("Added Series", series);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Added Series - {series.Title}");
        };

        _movieRepo.ItemRemoved += (sender, movie) =>
        {
            _movieAuditService.LogAudit("Removed Movie", movie);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Removed Movie - {movie.Title}");
        };

        _seriesRepo.ItemRemoved += (sender, series) =>
        {
            _seriesAuditService.LogAudit("Removed Series", series);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Removed Series - {series.Title}");
        };
    }

    public void Communication()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to the movie library!");
        Console.WriteLine("Select an option that interests you:");
        Console.WriteLine("Enter '1' if you want to display the entire contents of the library.");
        Console.WriteLine("Enter '2' if you want to remove an item from the library.");
        Console.WriteLine("Enter '3' if you want to add a movie to the library.");
        Console.WriteLine("Enter '4' if you want to add a series to the library.");
        Console.WriteLine("Enter 'q' if you want to quit.");

        while (true)
        {
            Console.WriteLine("Select another option:");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    WriteAllToConsole(_movieRepo, _seriesRepo);
                    break;
                case "2":
                    RemoveItemFromLibrary(_movieRepo, _seriesRepo);
                    break;
                case "3":
                    AddMovie(_movieRepo);
                    break;
                case "4":
                    AddSeries(_seriesRepo);
                    break;
                case "q":
                    _jsonMovieService.SaveToFile(_movieRepo.GetAll());
                    _jsonSeriesService.SaveToFile(_seriesRepo.GetAll());
                    return;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
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
    }
}

using MoviesLibraryApp.DataProviders;
using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Repositories;
using MoviesLibraryApp.Services;

namespace MoviesLibraryApp
{
    public class App : IApp
    {
        private readonly IRepository<Movie> _movieRepo;
        private readonly IRepository<Series> _seriesRepo;
        private readonly IJsonFileService<Movie> _jsonMovieService;
        private readonly IJsonFileService<Series> _jsonSeriesService;
        private readonly IAuditService<Movie> _movieAuditService;
        private readonly IAuditService<Series> _seriesAuditService;
        private readonly IMovieProvider _movieProvider;
        private readonly ISeriesProvider _seriesProvider;

        public App(IRepository<Series> seriesRepo,
            IRepository<Movie> movieRepo,
            IJsonFileService<Movie> jsonMovieService,
            IJsonFileService<Series> jsonSeriesService,
            IAuditService<Movie> movieAuditService,
            IAuditService<Series> seriesAuditService,
            IMovieProvider movieProvider,
            ISeriesProvider seriesProvider)
        {
            _seriesRepo = seriesRepo;
            _movieRepo = movieRepo;
            _jsonMovieService = jsonMovieService;
            _jsonSeriesService = jsonSeriesService;
            _movieAuditService = movieAuditService;
            _seriesAuditService = seriesAuditService;
            _movieProvider = movieProvider;
            _seriesProvider = seriesProvider;
        }
        
        public void Run()
        {
            LoadDataFromFiles();

            DisplayMovieInformation();
            DisplaySeriesInformation();
        }

        private void LoadDataFromFiles()
        {
            var moviesFromFile = _jsonMovieService.LoadFromFile();
            var seriesFromFile = _jsonSeriesService.LoadFromFile();

            if (moviesFromFile.Any())
            {
                foreach (var movie in moviesFromFile)
                {
                    _movieRepo.Add(movie);
                }
            }

            if (seriesFromFile.Any())
            {
                foreach (var series in seriesFromFile)
                {
                    _seriesRepo.Add(series);
                }
            }

            if (!_movieRepo.GetAll().Any() && !_seriesRepo.GetAll().Any())
            {
                AddInitialMoviesAndSeries(_movieRepo, _seriesRepo);
            }

            _movieRepo.Save();
            _seriesRepo.Save();
        }
        private void AddInitialMoviesAndSeries(IRepository<Movie> movieRepo, IRepository<Series> seriesRepo)
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Green Mile", Type = "Drama" },
                new Movie { Title = "Forrest Gump", Type = "Comedy" },
                new Movie { Title = "Joker", Type = "Thriller" },
                new Movie { Title = "Avatar", Type = "Fantasy" },
                new Movie { Title = "The Godfather", Type = "Drama" },
                new Movie { Title = "Kac Vegas", Type = "Comedy" },
                new Movie { Title = "Anabelle", Type = "Horror" },
                new Movie { Title = "Anyone but you", Type = "Romantic comedy" }
            };

            var series = new List<Series>
            {
                new Series { Title = "Breaking Bad", Type = "Crime" },
                new Series { Title = "The Crown", Type = "Historical" },
                new Series { Title = "The Watcher", Type = "Horror" },
                new Series { Title = "Forst", Type = "Crime" },
                new Series { Title = "1670", Type = "Comedy" },
                new Series { Title = "New Amsterdam", Type = "Drama" }
            };

            foreach (var movie in movies)
            {
                movieRepo.Add(movie);
                _movieAuditService.LogAudit("Initial Add Movie", movie);
            }

            foreach (var serie in series)
            {
                seriesRepo.Add(serie);
                _seriesAuditService.LogAudit("Initial Add Series", serie);
            }

            movieRepo.Save();
            seriesRepo.Save();  
        }
        private void DisplayMovieInformation()
        {
            Console.WriteLine();
            Console.WriteLine("GetUniqueMovieType:");
            foreach (var item in _movieProvider.GetUniqueMovieType())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine("OrderByMovieType:");
            foreach (var item in _movieProvider.OrderByMovieType())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine("FirstOrDefaultByMovieType");
            Console.WriteLine(_movieProvider.FirstOrDefaultByMovieType("Drama"));

            Console.WriteLine();
            Console.WriteLine("SingleOrDefaultByMovieId");
            Console.WriteLine(_movieProvider.SingleOrDefaultByMovieId(3));
        }

        private void DisplaySeriesInformation()
        {
            Console.WriteLine();
            Console.WriteLine("GetUniqueSeriesType:");
            foreach (var item in _seriesProvider.GetUniqueSeriesType())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine("OrderBySeriesType:");
            foreach (var item in _seriesProvider.OrderBySeriesType())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine("FirstOrDefaultBySeriesType");
            Console.WriteLine(_seriesProvider.FirstOrDefaultBySeriesType("Horror"));

            Console.WriteLine();
            Console.WriteLine("SingleOrDefaultBySeriesId");
            Console.WriteLine(_seriesProvider.SingleOrDefaultBySeriesId(4));
        }
    }
}

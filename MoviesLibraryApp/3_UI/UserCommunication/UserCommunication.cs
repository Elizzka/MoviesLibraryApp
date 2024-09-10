using MoviesLibraryApp.CsvReader;
using MoviesLibraryApp.Data;
using MoviesLibraryApp.Data.Entities;
using MoviesLibraryApp.Data.Repositories;
using MoviesLibraryApp.Services;

namespace MoviesLibraryApp.UserCommunication
{
    public class UserCommunication : IUserCommunication
    {
        private readonly IRepository<Series> _seriesRepo;
        private readonly IRepository<Movie> _movieRepo;
        private readonly IAuditService<Movie> _movieAuditService;
        private readonly IAuditService<Series> _seriesAuditService;
        private readonly ICsvReader _csvReader;
        private readonly MoviesLibraryAppDbContext _moviesLibraryAppDbContext;

        public UserCommunication(
            IRepository<Series> seriesRepo,
            IRepository<Movie> movieRepo,
            IAuditService<Movie> movieAuditService,
            IAuditService<Series> seriesAuditService,
            ICsvReader csvReader,
            MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            _seriesRepo = seriesRepo;
            _movieRepo = movieRepo;
            _movieAuditService = movieAuditService;
            _seriesAuditService = seriesAuditService;
            _csvReader = csvReader;
            _moviesLibraryAppDbContext = moviesLibraryAppDbContext;
            _moviesLibraryAppDbContext.Database.EnsureCreated();

            _movieRepo.ItemAdded += (sender, movie) => LogItemEvent("Added", movie, "Movie", movie.Title);
            _movieRepo.ItemRemoved += (sender, movie) => LogItemEvent("Removed", movie, "Movie", movie.Title);
            _movieRepo.ItemUpdated += (sender, movie) => LogItemEvent("Updated", movie, "Movie", movie.Title);

            _seriesRepo.ItemAdded += (sender, series) => LogItemEvent("Added", series, "Series", series.Title);
            _seriesRepo.ItemRemoved += (sender, series) => LogItemEvent("Removed", series, "Series", series.Title);
            _seriesRepo.ItemUpdated += (sender, series) => LogItemEvent("Updated", series, "Series", series.Title);
        }

        private void LogItemEvent<T>(string action, T item, string itemType, string title)
        {
            if (item is Movie movie)
            {
                _movieAuditService.LogAudit($"{action} {itemType}", movie);
            }
            else if (item is Series series)
            {
                _seriesAuditService.LogAudit($"{action} {itemType}", series);
            }

            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {action} {itemType} - {title}");
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
            Console.WriteLine("Enter '5' if you want insert data.");
            Console.WriteLine("Enter '6' if you want change data.");
            Console.WriteLine("Enter 'q' if you want to quit.");

            while (true)
            {
                Console.WriteLine("Select another option:");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        WriteAllToConsole(_moviesLibraryAppDbContext);
                        break;
                    case "2":
                        RemoveItemFromLibrary(_moviesLibraryAppDbContext);
                        break;
                    case "3":
                        AddMovie(_moviesLibraryAppDbContext);
                        break;
                    case "4":
                        AddSeries(_moviesLibraryAppDbContext);
                        break;
                    case "5":
                        InsertData(_moviesLibraryAppDbContext);
                        break;
                    case "6":
                        ChangeSelectedData(_moviesLibraryAppDbContext);
                        break;
                    case "q":
                        _moviesLibraryAppDbContext.SaveChanges();
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        public void WriteAllToConsole(MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            var moviesFromDb = _movieRepo.GetAll().ToList();

            Console.WriteLine("Movies:");
            foreach (var movie in moviesFromDb)
            {
                Console.WriteLine($"\t{movie.Id}, {movie.Title}, {movie.Year}, {movie.Director}, {movie.Type}");
            }

            var seriesFromDb = _seriesRepo.GetAll().ToList();

            Console.WriteLine("\nSeries:");
            foreach (var serie in seriesFromDb)
            {
                Console.WriteLine($"\t{serie.Id}, {serie.Title}, {serie.Year}, {serie.Director}, {serie.Type}");
            }
        }

        public void RemoveItemFromLibrary(MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            Console.WriteLine("Enter a title for the item you want to delete:");
            var title = Console.ReadLine();

            var movie = _movieRepo.GetAll().FirstOrDefault(x => x.Title == title);
            if (movie != null)
            {
                _movieRepo.Remove(movie);
                _movieRepo.Save();
            }
            else
            {
                var series = _seriesRepo.GetAll().FirstOrDefault(x => x.Title == title);
                if (series != null)
                {
                    _seriesRepo.Remove(series);
                    _seriesRepo.Save();
                }
                else
                {
                    Console.WriteLine($"No item with title '{title}' was found.");
                }
            }
        }

        public void AddMovie(MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            Console.WriteLine("Enter the title of the movie you want to add:");
            var title = Console.ReadLine();

            var existingMovie = _movieRepo.GetAll().FirstOrDefault(m => m.Title == title);
            if (existingMovie != null)
            {
                Console.WriteLine("Movie with this title already exists.");
                return;
            }

            Console.WriteLine("Enter the year of the movie:");
            var year = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the director of the movie:");
            var director = Console.ReadLine();
            Console.WriteLine("Enter specify the type of the movie:");
            var type = Console.ReadLine();

            _movieRepo.Add(new Movie
            {
                Title = title,
                Year = year,
                Director = director,
                Type = type
            });

            _movieRepo.Save();
        }

        public void AddSeries(MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            Console.WriteLine("Enter the title of the series you want to add:");
            var title = Console.ReadLine();

            var existingSeries = _seriesRepo.GetAll().FirstOrDefault(m => m.Title == title);
            if (existingSeries != null)
            {
                Console.WriteLine("Series with this title already exists.");
                return;
            }
            Console.WriteLine("Enter the year of the series:");
            var year = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the director of the series:");
            var director = Console.ReadLine();
            Console.WriteLine("Enter specify the type of the series:");
            var type = Console.ReadLine();

            _seriesRepo.Add(new Series
            {
                Title = title,
                Year = year,
                Director = director,
                Type = type
            });

            _seriesRepo.Save();
        }

        private void InsertData(MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            var productions = _csvReader.ProcessProductions("C:\\Users\\elizz\\OneDrive\\Pulpit\\Projekty P\\MoviesLibraryApp\\MoviesLibraryApp\\1_DataAccess\\Resources\\Files\\production.csv");

            foreach (var production in productions)
            {
                if (production.MovieOrSeries == "Movie")
                {
                    bool movieExists = _movieRepo.GetAll()
                                        .Any(m => m.Title == production.Title && m.Year == production.Year);

                    if (!movieExists)
                    {
                        _movieRepo.Add(new Movie
                        {
                            Title = production.Title,
                            Year = production.Year,
                            Director = production.Director,
                            Type = production.Type
                        });

                        _movieRepo.Save();
                    }
                }
                else if (production.MovieOrSeries == "Series")
                {
                    bool seriesExists = _seriesRepo.GetAll()
                                        .Any(s => s.Title == production.Title && s.Year == production.Year);

                    if (!seriesExists)
                    {
                        _seriesRepo.Add(new Series
                        {
                            Title = production.Title,
                            Year = production.Year,
                            Director = production.Director,
                            Type = production.Type
                        });

                        _seriesRepo.Save();
                    }
                }
            }
        }

        public void ChangeSelectedData(MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            Console.WriteLine("Enter the title of the movie or series you want to change:");
            var title = Console.ReadLine();

            var movie = _movieRepo.GetAll().FirstOrDefault(x => x.Title == title);
            var series = _seriesRepo.GetAll().FirstOrDefault(x => x.Title == title);

            if (movie != null || series != null)
            {
                object itemToUpdate = movie ?? (object)series;

                ShowChangeOptions();

                char input = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                switch (input)
                {
                    case 'T':
                        ChangeTitle(itemToUpdate);
                        break;
                    case 'Y':
                        ChangeYear(itemToUpdate);
                        break;
                    case 'D':
                        ChangeDirector(itemToUpdate);
                        break;
                    case 'K':
                        ChangeType(itemToUpdate);
                        break;
                    default:
                        Console.WriteLine("Wrong option selected.");
                        return;
                }

                if (itemToUpdate is Movie updatedMovie)
                {
                    _movieRepo.Update(updatedMovie);
                    _movieRepo.Save();
                }
                else if (itemToUpdate is Series updatedSeries)
                {
                    _seriesRepo.Update(updatedSeries);
                    _seriesRepo.Save();
                }     
            }
            else
            {
                Console.WriteLine($"No movie or series with title '{title}' was found.");
            }

            void ShowChangeOptions()
            {
                Console.WriteLine("Choose what you want to change:");
                Console.WriteLine("Choose 'T' if you want to change the title:");
                Console.WriteLine("Choose 'Y' if you want to change the year:");
                Console.WriteLine("Choose 'D' if you want to change the director:");
                Console.WriteLine("Choose 'K' if you want to change the type:");
            }

            void ChangeTitle(object item)
            {
                Console.WriteLine("Enter the new title:");
                string newTitle = Console.ReadLine();
                if (item is Movie movie) movie.Title = newTitle;
                else if (item is Series series) series.Title = newTitle;
            }

            void ChangeYear(object item)
            {
                Console.WriteLine("Enter the new year:");
                int newYear = int.Parse(Console.ReadLine());
                if (item is Movie movie) movie.Year = newYear;
                else if (item is Series series) series.Year = newYear;
            }

            void ChangeDirector(object item)
            {
                Console.WriteLine("Enter the new director:");
                string newDirector = Console.ReadLine();
                if (item is Movie movie) movie.Director = newDirector;
                else if (item is Series series) series.Director = newDirector;
            }

            void ChangeType(object item)
            {
                Console.WriteLine("Enter the new type:");
                string newType = Console.ReadLine();
                if (item is Movie movie) movie.Type = newType;
                else if (item is Series series) series.Type = newType;
            }
        }
    }
}

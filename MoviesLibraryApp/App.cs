﻿using MoviesLibraryApp.Components.CsvReader;
using MoviesLibraryApp.Components.CsvReader.Models;
using MoviesLibraryApp.Components.DataProviders;
using MoviesLibraryApp.Data;
using MoviesLibraryApp.Data.Entities;
using MoviesLibraryApp.Data.Repositories;
using MoviesLibraryApp.Services;
using System.Xml.Linq;

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
        private readonly ICsvReader _csvReader;
        private readonly MoviesLibraryAppDbContext _moviesLibraryAppDbContext;

        public App(IRepository<Series> seriesRepo,
            IRepository<Movie> movieRepo,
            IJsonFileService<Movie> jsonMovieService,
            IJsonFileService<Series> jsonSeriesService,
            IAuditService<Movie> movieAuditService,
            IAuditService<Series> seriesAuditService,
            IMovieProvider movieProvider,
            ISeriesProvider seriesProvider,
            ICsvReader csvReader,
            MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            _seriesRepo = seriesRepo;
            _movieRepo = movieRepo;
            _jsonMovieService = jsonMovieService;
            _jsonSeriesService = jsonSeriesService;
            _movieAuditService = movieAuditService;
            _seriesAuditService = seriesAuditService;
            _movieProvider = movieProvider;
            _seriesProvider = seriesProvider;
            _csvReader = csvReader;
            _moviesLibraryAppDbContext = moviesLibraryAppDbContext;
            _moviesLibraryAppDbContext.Database.EnsureCreated();
        }
        
        public void Run()
        {
            ChangeSelectedData();
            ReadGroupedProductionsFromDb();
            DisplayAllProductionsFromDb();
            //InsertData();
            DisplayProductionsDirectors();
            CsvToXml();
            QueryXml();
            LoadDataFromFiles();
            DisplayMovieInformation();
            DisplaySeriesInformation();
        }

        public void ChangeSelectedData()
        {
            var inception = this.ReadFirst("Inception");
            inception.Title = "New title!!";
            _moviesLibraryAppDbContext.SaveChanges();
        }

        private Movie? ReadFirst(string title)
        {
            return _moviesLibraryAppDbContext.Movies.FirstOrDefault(x => x.Title == title);
        }
        private void ReadGroupedProductionsFromDb()
        {
            Console.WriteLine("Movies grouped by type:");
            Console.WriteLine("************************");

            var groupsMovies = _moviesLibraryAppDbContext
                .Movies
                .GroupBy(x => x.Type)
                .Select(x => new
                {
                    Type = x.Key,
                    Movies = x.ToList()
                })
                .ToList();

            foreach(var group in groupsMovies)
            {
                Console.WriteLine(group.Type);
                foreach(var movies in group.Movies)
                {
                    Console.WriteLine($"\t{movies.Title}, {movies.Year}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Series grouped by type:");
            Console.WriteLine("************************");

            var groupsSeries = _moviesLibraryAppDbContext
                .Series
                .GroupBy(x => x.Type)
                .Select(x => new
                {
                    Type = x.Key,
                    Series = x.ToList()
                })
                .ToList();

            foreach (var group in groupsSeries)
            {
                Console.WriteLine(group.Type);
                foreach (var series in group.Series)
                {
                    Console.WriteLine($"\t{series.Title}, {series.Year}");
                }
                Console.WriteLine();
            }
        }

        public void DisplayAllProductionsFromDb()
        {
            var moviesFromDb = _moviesLibraryAppDbContext.Movies.ToList();

            foreach(var movieFromDb in moviesFromDb)
            {
                Console.WriteLine($"\t{movieFromDb.Title}, {movieFromDb.Year}");
            }

            var seriesFromDb = _moviesLibraryAppDbContext.Series.ToList();

            foreach (var serieFromDb in seriesFromDb)
            {
                Console.WriteLine($"\t{serieFromDb.Title}, {serieFromDb.Year}");
            }

        }
        private void InsertData()
        {
            var productions = _csvReader.ProcessProductions("C:\\Users\\elizz\\OneDrive\\Pulpit\\Projekty P\\MoviesLibraryApp\\MoviesLibraryApp\\Resources\\Files\\production.csv");

            foreach (var production in productions)
            {
                if (production.MovieOrSeries == "Movie")
                {
                    _moviesLibraryAppDbContext.Movies.Add(new Movie()
                    {
                        Title = production.Title,
                        Year = production.Year,
                        Director = production.Director,
                        Type = production.Type
                    });
                }
                else if (production.MovieOrSeries == "Series")
                {
                    _moviesLibraryAppDbContext.Series.Add(new Series()
                    {
                        Title = production.Title,
                        Year = production.Year,
                        Director = production.Director,
                        Type = production.Type
                    });
                }
            }
                _moviesLibraryAppDbContext.SaveChanges();
        }

        private void CsvToXml()
        {
            var records = _csvReader.ProcessProductions("C:\\Users\\elizz\\OneDrive\\Pulpit\\Projekty P\\MoviesLibraryApp\\MoviesLibraryApp\\Resources\\Files\\production.csv");
            var document = new XDocument();

            var productions = new XElement("Productions", records
                .Select(x =>
                new XElement("Production",
                new XAttribute("Title", x.Title),
                new XAttribute("Year", x.Year),
                new XAttribute("Director", x.Director),
                new XAttribute("Type", x.Type),
                new XAttribute("MovieOrSeries", x.MovieOrSeries))));

            document.Add(productions);
            document.Save("production.xml");
        }

        private static void QueryXml()
        {
            var document = XDocument.Load("production.xml");
            var titles = document
                .Elements("Productions")?
                .Elements("Production")
                .Where(x => x.Attribute("Type")?.Value == "Drama")
                .Select(x => x.Attribute("Title")?.Value);

            Console.WriteLine();
            foreach (var title in titles)
            {
                Console.WriteLine(title);
            }
        }

        private void DisplayProductionsDirectors()
        {
            var directors = _csvReader.ProcessDirectors("C:\\Users\\elizz\\OneDrive\\Pulpit\\Projekty P\\MoviesLibraryApp\\MoviesLibraryApp\\Resources\\Files\\director.csv");
            var productions = _csvReader.ProcessProductions("C:\\Users\\elizz\\OneDrive\\Pulpit\\Projekty P\\MoviesLibraryApp\\MoviesLibraryApp\\Resources\\Files\\production.csv");

            var productionsDirectors = productions.Join(
                directors, 
                x => x.Director,
                x => x.Name,
                (production, director) =>
                new
                {
                    production.Director,
                    director.YearOfBirth,
                    director.CountryOfBirth,
                    production.Title
                })
                .OrderByDescending(x => x.CountryOfBirth)
                .ThenBy(x => x.YearOfBirth);

            foreach (var production in productionsDirectors)
            {
                Console.WriteLine($"Country Of Birth: {production.CountryOfBirth}");
                Console.WriteLine($"\t Director: {production.Director}");
                Console.WriteLine($"\t Year Of Birth: {production.YearOfBirth}");
                Console.WriteLine($"\t Title: {production.Title}");
            }
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

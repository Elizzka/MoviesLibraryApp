﻿using Microsoft.EntityFrameworkCore;
using MoviesLibraryApp.CsvReader;
using MoviesLibraryApp.Data;
using MoviesLibraryApp.Data.Entities;
using MoviesLibraryApp.Data.Repositories;
using MoviesLibraryApp.Services;

namespace MoviesLibraryApp
{
    public class App : IApp
    {
        private readonly IRepository<Movie> _movieRepo;
        private readonly IRepository<Series> _seriesRepo;
        private readonly IAuditService<Movie> _movieAuditService;
        private readonly IAuditService<Series> _seriesAuditService;
        private readonly ICsvReader _csvReader;
        private readonly MoviesLibraryAppDbContext _moviesLibraryAppDbContext;

        public App(IRepository<Series> seriesRepo,
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
        }

        public void Run()
        {
            LoadDataFromFiles(_moviesLibraryAppDbContext);
        }

        private void LoadDataFromFiles(MoviesLibraryAppDbContext moviesLibraryAppDbContext)
        {
            AddInitialData<Movie>(_moviesLibraryAppDbContext.Movies, _movieAuditService, GetInitialMovies(), "Initial Add Movie");
            AddInitialData<Series>(_moviesLibraryAppDbContext.Series, _seriesAuditService, GetInitialSeries(), "Initial Add Series");
        }

        private void AddInitialData<T>(DbSet<T> dbSet, IAuditService<T> auditService, List<T> items, string auditMessage) where T : class
        {
            if (!dbSet.Any())
            {
                foreach (var item in items)
                {
                    dbSet.Add(item);
                    auditService.LogAudit(auditMessage, item);
                }

                _moviesLibraryAppDbContext.SaveChanges();
            }
        }

        private List<Movie> GetInitialMovies()
        {
            return new List<Movie>
            {
                new Movie { Title = "The Green Mile", Year = 1999 , Director = "Frank Darabont", Type = "Drama" },
                new Movie { Title = "Forrest Gump", Year = 1994 , Director = "Robert Zemeckis", Type = "Comedy"},
                new Movie { Title = "Joker", Year = 2019 , Director = "Todd Phillips", Type = "Thriller"},
                new Movie { Title = "Avatar", Year = 2009 , Director = "James Cameron", Type = "Fantasy" },
                new Movie { Title = "The Godfather", Year = 1972, Director = "Francis Ford Coppola", Type = "Drama" },
                new Movie { Title = "Kac Vegas", Year = 2009 ,Director = "Todd Phillips", Type = "Comedy"},
                new Movie { Title = "Anabelle", Year = 2014 ,Director = "John R. Leonetti", Type = "Horror" },
                new Movie { Title = "Anyone but you", Year = 2024 ,Director = "Will Gluck", Type = "Romantic comedy" }
            };
        }

        private List<Series> GetInitialSeries()
        {
            return new List<Series>
            {
                new Series { Title = "Breaking Bad", Year = 2008, Director = "Vince Gilligan",  Type = "Crime" },
                new Series { Title = "The Crown", Year = 2016, Director = "Peter Morgan",  Type = "Historical" },
                new Series { Title = "The Watcher", Year = 2022, Director = "Ryan Murphy",  Type = "Horror" },
                new Series { Title = "New Amsterdam", Year = 2018, Director = "David Schulner",  Type = "Drama"}
            };
        }
    }
}

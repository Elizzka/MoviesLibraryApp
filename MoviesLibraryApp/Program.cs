using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibraryApp;
using MoviesLibraryApp.Components.CsvReader;
using MoviesLibraryApp.Components.DataProviders;
using MoviesLibraryApp.Data;
using MoviesLibraryApp.Data.Entities;
using MoviesLibraryApp.Data.Repositories;
using MoviesLibraryApp.Services;
using MoviesLibraryApp.UserCommunication;


var services = new ServiceCollection();
services.AddSingleton<IApp, App>();
services.AddSingleton<IRepository<Movie>, ListRepository<Movie>>();
services.AddSingleton<IRepository<Series>, ListRepository<Series>>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IJsonFileService<Movie>>(new JsonFileService<Movie>("movies.json"));
services.AddSingleton<IJsonFileService<Series>>(new JsonFileService<Series>("series.json"));
services.AddSingleton<IAuditService<Movie>>(new AuditService<Movie>("auditLogMovies.txt"));
services.AddSingleton<IAuditService<Series>>(new AuditService<Series>("auditLogSeries.txt"));
services.AddSingleton<IMovieProvider, MovieProvider>();
services.AddSingleton<ISeriesProvider, SeriesProvider>();
services.AddSingleton<ICsvReader, CsvReader>();
services.AddDbContext<MoviesLibraryAppDbContext>(options => options
.UseSqlServer("Data Source=DESKTOP-EEF7JMA\\SQLEXPRESS01;Initial Catalog=MoviesLibraryStorage;Integrated Security=True;Trust Server Certificate=True"));

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IApp>()!;
app.Run();
var appCom = serviceProvider.GetService<IUserCommunication>()!;
appCom.Communication();

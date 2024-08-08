using Microsoft.Extensions.DependencyInjection;
using MoviesLibraryApp;
using MoviesLibraryApp.DataProviders;
using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Repositories;
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

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IApp>()!;
app.Run();
var appCom = serviceProvider.GetService<IUserCommunication>()!;
appCom.Communication();


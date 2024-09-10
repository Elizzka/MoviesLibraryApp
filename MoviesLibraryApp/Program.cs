using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibraryApp;
using MoviesLibraryApp.CsvReader;
using MoviesLibraryApp.Data;
using MoviesLibraryApp.Data.Entities;
using MoviesLibraryApp.Data.Repositories;
using MoviesLibraryApp.Services;
using MoviesLibraryApp.UserCommunication;

var services = new ServiceCollection();
services.AddSingleton<IApp, App>();
services.AddSingleton<IRepository<Movie>, SqlRepository<Movie>>();
services.AddSingleton<IRepository<Series>, SqlRepository<Series>>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IAuditService<Movie>>(new AuditService<Movie>("auditLogMovies.txt"));
services.AddSingleton<IAuditService<Series>>(new AuditService<Series>("auditLogSeries.txt"));
services.AddSingleton<ICsvReader, CsvReader>();
services.AddDbContext<MoviesLibraryAppDbContext>(options => options
.UseSqlServer("Data Source=DESKTOP-EEF7JMA\\SQLEXPRESS01;Initial Catalog=MoviesLibraryStorage;Integrated Security=True;Trust Server Certificate=True"));

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IApp>()!;
app.Run();
var appCom = serviceProvider.GetService<IUserCommunication>()!;
appCom.Communication();

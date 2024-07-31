using MoviesLibraryApp.Repositories;
using MoviesLibraryApp.Entities;
using MoviesLibraryApp.Data;
using MoviesLibraryApp.Repositories.Extensions;

Console.WriteLine("Welcome to the movie library!");
Console.WriteLine("Select an option that interests you:");
Console.WriteLine("Enter '1' if you want to display the entire contents of the library.");
Console.WriteLine("Enter '2' if you want to remove an item from the library.");
Console.WriteLine("Enter '3' if you want to add a movie to the library.");
Console.WriteLine("Enter '4' if you want to add a series to the library.");
Console.WriteLine("Enter 'q' if you want to quit.");

var movieRepository = new SqlRepository<Movie>(new MoviesLibraryAppDbContext());    

if (!movieRepository.GetAll().Any())
{
    AddInitialMoviesAndSeries(movieRepository);
}

movieRepository.ItemAdded += MovieRepositoryOnItemAdded;

while (true)
{
    Console.WriteLine("Select another option:");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            WriteAllToConsole(movieRepository);
            break;
        case "2":
            RemoveItemFromLibrary(movieRepository);
            break;
        case "3":
            AddMovie(movieRepository);
            break;
        case "4":
            AddSeries(movieRepository);
            break;
        case "q":
            return;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}

static void MovieRepositoryOnItemAdded(object? sender, Movie e)
{
    var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    Console.WriteLine($"{data} - Added - {e.Title}");
}

static void AddInitialMoviesAndSeries(IRepository<Movie> repository)
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

    repository.AddBatch(movies.ToArray());
    repository.AddBatch(series.ToArray());
}

static void WriteAllToConsole(IReadRepository<IEntity> repository)
{
    var items = repository.GetAll();
    foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}

static void RemoveItemFromLibrary(IRepository<Movie> repository)
{
    Console.WriteLine("Enter a title for the item you want to delete:");
    var title = Console.ReadLine();

    var item = repository.GetAll().FirstOrDefault(x => x.Title == title);
    if (item != null)
    {
        repository.Remove(item);
        Console.WriteLine($"The item '{title}' has been removed.");
    }
    else
    {
        Console.WriteLine($"No item with title '{title}' was found.");
    }
}

static void AddMovie(IRepository<Movie> repository)
{
    Console.WriteLine("Enter the title of the video you want to add:");
    var title = Console.ReadLine();

    Console.WriteLine("Please specify the type of video:");
    var type = Console.ReadLine();

    var movie = new Movie { Title = title, Type = type };
    repository.Add(movie);

    //Console.WriteLine($"The video '{title}' has been added to the library.");
}

static void AddSeries(IRepository<Movie> repository)
{
    Console.WriteLine("Enter the title of the series you want to add:");
    var title = Console.ReadLine();

    Console.WriteLine("Please specify the type of series:");
    var type = Console.ReadLine();

    var series = new Series { Title = title, Type = type };
    repository.Add(series);

    Console.WriteLine($"The series '{title}' has been added to the library.");
}

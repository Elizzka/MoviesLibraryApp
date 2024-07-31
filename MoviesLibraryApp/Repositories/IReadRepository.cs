using MoviesLibraryApp.Entities;

namespace MoviesLibraryApp.Repositories;

public interface IReadRepository<out T> where T : class, IEntity
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
}

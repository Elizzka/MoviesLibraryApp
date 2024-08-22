using MoviesLibraryApp.Data.Entities;

namespace MoviesLibraryApp.Data.Repositories;

public interface IReadRepository<out T> where T : class, IEntity
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
}

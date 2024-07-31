using MoviesLibraryApp.Entities;

namespace MoviesLibraryApp.Repositories;

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : class, IEntity
{
}

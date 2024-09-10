using MoviesLibraryApp.Data.Entities;

namespace MoviesLibraryApp.Data.Repositories
{
    public interface IWriteRepository<in T> where T : class, IEntity
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        void Save();
    }
}

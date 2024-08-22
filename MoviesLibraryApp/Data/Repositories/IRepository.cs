using MoviesLibraryApp.Data.Entities;
using System;

namespace MoviesLibraryApp.Data.Repositories;

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : class, IEntity
{
    event EventHandler<T> ItemAdded;
    event EventHandler<T> ItemRemoved;

    void Add(T item);
    void Remove(T item);
    void Save();
}

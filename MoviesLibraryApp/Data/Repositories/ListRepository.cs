namespace MoviesLibraryApp.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using MoviesLibraryApp.Data.Entities;
    using System.Collections.Generic;

    public class ListRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly MoviesLibraryAppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;
        public event EventHandler<T>? ItemUpdated;

        public ListRepository(MoviesLibraryAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
            _dbSet = _dbContext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            _dbContext.SaveChanges();
            ItemAdded?.Invoke(this, item);
        }

        public T GetById(int id)
        {
            return _dbSet.Single(item => item.Id == id);
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
            _dbContext.SaveChanges();
            ItemRemoved?.Invoke(this, item);
        }

        public void Update(T item)
        {
            _dbSet.Update(item);
            _dbContext.SaveChanges();
            ItemUpdated?.Invoke(this, item);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MoviesLibraryApp.Data.Entities;

namespace MoviesLibraryApp.Data.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly DbSet<T> _dbSet;
        private readonly MoviesLibraryAppDbContext _dbContext;
        private readonly Action<T>? _itemAddedCallback;

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T> ItemRemoved;
        public event EventHandler<T> ItemUpdated;

        public SqlRepository(MoviesLibraryAppDbContext dbContext, Action<T>? itemAddedCallback = null)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
            _dbSet = _dbContext.Set<T>();
            _itemAddedCallback = itemAddedCallback;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.OrderBy(item => item.Id).ToList();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            _dbContext.SaveChanges();
            ItemAdded?.Invoke(this, item);
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
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


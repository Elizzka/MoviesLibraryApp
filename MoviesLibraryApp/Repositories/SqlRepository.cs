using Microsoft.EntityFrameworkCore;
using MoviesLibraryApp.Entities;

namespace MoviesLibraryApp.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;
        private readonly Action<T>? _itemAddedCallback;

        public SqlRepository(DbContext dbContext, Action<T>? itemAddedCallback = null)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _itemAddedCallback = itemAddedCallback;
        }

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T> ItemRemoved;

        public IEnumerable<T> GetAll()
        {
            return _dbSet.OrderBy(item => item.Id).ToList();
        }

        public void Add(T item)
        {
            if (!_dbSet.Any(e => e.Id == item.Id))
            {
                _dbSet.Add(item);
                _itemAddedCallback?.Invoke(item);
                ItemAdded?.Invoke(this, item);
                _dbContext.SaveChanges();
            }
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

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}


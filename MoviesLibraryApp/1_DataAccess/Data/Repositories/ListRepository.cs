namespace MoviesLibraryApp.Data.Repositories
{
    using MoviesLibraryApp.Data.Entities;
    using System.Collections.Generic;

    public class ListRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly List<T> _items = new List<T>();

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;
        public event EventHandler<T>? ItemUpdated;

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public void Add(T item)
        {
            _items.Add(item);
            ItemAdded?.Invoke(this, item);
        }

        public T GetById(int id)
        {
            return _items.Single(item => item.Id == id);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
            ItemRemoved?.Invoke(this, item);
        }

        public void Update(T item)
        {
            var index = _items.FindIndex(i => i.Id == item.Id);
            ItemUpdated?.Invoke(this, item);
        }

        public void Save()
        {
        }
    }
}

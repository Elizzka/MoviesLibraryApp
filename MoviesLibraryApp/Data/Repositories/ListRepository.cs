namespace MoviesLibraryApp.Data.Repositories
{
    using MoviesLibraryApp.Data.Entities;
    using System.Collections.Generic;

    public class ListRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly List<T> _items = new();

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;
        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public void Add(T item)
        {
            item.Id = _items.Count + 1;
            _items.Add(item);
            OnItemAdded(item);
        }

        public T GetById(int id)
        {
            return _items.Single(item => item.Id == id);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
            OnItemRemoved(item);
        }

        public void Save()
        {
        }

        protected virtual void OnItemAdded(T item)
        {
            ItemAdded?.Invoke(this, item);
        }

        protected virtual void OnItemRemoved(T item)
        {
            ItemRemoved?.Invoke(this, item);
        }
    }
}

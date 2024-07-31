using Microsoft.EntityFrameworkCore;
using MoviesLibraryApp.Entities;
using System.Text.Json;

namespace MoviesLibraryApp.Repositories;

public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
{
    private const string fileName = "MoviesLibrary.json";
    private const string auditFileName = "AuditLog.txt";
    private readonly DbSet<T> _dbSet;
    private readonly DbContext _dbContext;
    private readonly Action<T>? _itemAddedCallback;

    public SqlRepository(DbContext dbContext, Action<T>? itemAddedCallback = null)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
        _itemAddedCallback = itemAddedCallback; 
        LoadFromFile();
    }

    public event EventHandler<T>? ItemAdded;
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
            SaveToFile();
            LogAudit("Added", item);
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
        SaveToFile();
        LogAudit("Removed", item);
    }

    private void LogAudit(string action, T item)
    {
        var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logEntry = $"{data} - {action} - {item}";
        try
        {
            File.AppendAllText(auditFileName, logEntry + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to log audit entry: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        if (File.Exists(fileName))
        {
            var json = File.ReadAllText(fileName);
            var items = JsonSerializer.Deserialize<List<T>>(json);

            if (items != null)
            {
                foreach (var item in items)
                {
                    if (!_dbSet.Any(e => e.Id == item.Id))
                    {
                        _dbSet.Add(item);
                    }
                }
                _dbContext.SaveChanges();
            }
        }
    }

    private void SaveToFile()
    {
        var items = _dbSet.ToList();
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }

    public void Save()
    {
        _dbContext.SaveChanges();
        SaveToFile();
    }
}

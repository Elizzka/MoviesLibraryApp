namespace MoviesLibraryApp.Services
{
    public interface IAuditService<T>
    {
        void LogAudit(string action, T item);
    }
}


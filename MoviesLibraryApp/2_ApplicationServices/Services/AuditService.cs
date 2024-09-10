namespace MoviesLibraryApp.Services
{
    public class AuditService<T> : IAuditService<T>
    {
        private readonly string _auditFileName;

        public AuditService(string auditFileName)
        {
            _auditFileName = auditFileName;
        }

        public void LogAudit(string action, T item)
        {
            var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var logEntry = $"{data} - {action} - {item}";
            try
            {
                File.AppendAllText(_auditFileName, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log audit entry: {ex.Message}");
            }
        }
    }
}


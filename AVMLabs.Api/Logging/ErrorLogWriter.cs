namespace AVMLabs.Api.Logging
{
    public class ErrorLogWriter
    {
        private readonly IWebHostEnvironment _environment;

        public ErrorLogWriter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task WriteAsync(Exception exception, string? requestPath = null)
        {
            try
            {
                string logDirectory = Path.Combine(_environment.ContentRootPath, "Logs");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string fileName = $"Error_{DateTime.Now:yyyy-MM-dd}.log";
                string filePath = Path.Combine(logDirectory, fileName);

                string logEntry = $"""
                    ==================================================
                    Date       : {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                    Request    : {requestPath}
                    Exception  : {exception.GetType().FullName}
                    Message    : {exception.Message}
                    StackTrace :
                    {exception.StackTrace}
                    InnerError :
                    {exception.InnerException?.Message}
                    ==================================================

                    """;

                await File.AppendAllTextAsync(filePath, logEntry);
            }
            catch (Exception logException)
            {
                Console.Error.WriteLine($"Failed to write error log: {logException.Message}");
            }
        }
    }
}
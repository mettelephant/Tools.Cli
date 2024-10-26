using Tools.Cli.Logging.Help;

namespace Tools.Cli.Logging;

public class CompositeLogger(params ILogger[] loggers) : ILogger
{
    private readonly IEnumerable<ILogger> _loggers = loggers;


    public void Log(LogEntry logEntry)
    {
        foreach (var logger in _loggers)
        {
            logger.Log(logEntry);
        }
    }

    public void Log(LogLevel level, string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        foreach (var logger in _loggers)
        {
            logger.Log(level, message, file, data, sourceExtension, destinationExtension, helpExplanation);
        }
    }

    public void LogDebug(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        foreach (var logger in _loggers)
        {
            logger.LogDebug(message, file, data, sourceExtension, destinationExtension, helpExplanation);
        }
    }

    public void LogInformation(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        foreach (var logger in _loggers)
        {
            logger.LogInformation(message, file, data, sourceExtension, destinationExtension, helpExplanation);
        }
    }

    public void LogWarning(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        foreach (var logger in _loggers)
        {
            logger.LogWarning(message, file, data, sourceExtension, destinationExtension, helpExplanation);
        }
    }

    public void LogSummary(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        foreach (var logger in _loggers)
        {
            logger.LogSummary(message, file, data, sourceExtension, destinationExtension, helpExplanation);
        }
    }

    public void LogError(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        foreach (var logger in _loggers)
        {
            logger.LogError(message, file, data, sourceExtension, destinationExtension, helpExplanation);
        }
    }

    public void StartTask(string taskName, double maxValue = 100)
    {
        foreach (var logger in _loggers)
        {
            logger.StartTask(taskName);
        }
    }

    public void ReportProgress(string taskName, double value)
    {
        foreach (var logger in _loggers)
        {
            logger.ReportProgress(taskName, value);
        }
    }

    public void CompleteTask(string taskName)
    {
        foreach (var logger in _loggers)
        {
            logger.CompleteTask(taskName);
        }
    }
}
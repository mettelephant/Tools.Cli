using Tools.Cli.Logging.Help;

namespace Tools.Cli.Logging;

public static class Logger
{
    public static ILogger? Instance { get; set; }

    public static void Log(LogLevel level, string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = HelpExplanation.None)
    {
        var entry = new LogEntry
        {
            Level = level,
            Message = message,
            File = file,
            Data = data,
            SourceExtension = sourceExtension,
            DestinationExtension = destinationExtension,
            HelpExplanation = helpExplanation
        };
        Instance?.Log(entry);
    }

    public static void Log(LogEntry entry)
    {
        Instance?.Log(entry);
    }

    public static void LogDebug(string message) => Instance?.LogDebug(message);
    public static void LogInformation(string message) => Instance?.LogInformation(message);
    public static void LogSummary(string message) => Instance?.LogSummary(message);
    public static void LogWarning(string message) => Instance?.LogWarning(message);
    public static void LogError(string message) => Instance?.LogError(message);
    public static void StartTask(string taskName) => Instance?.StartTask(taskName);
    public static void ReportProgress(string taskName, int progress) => Instance?.ReportProgress(taskName, progress);
    public static void CompleteTask(string taskName) => Instance?.CompleteTask(taskName);
}
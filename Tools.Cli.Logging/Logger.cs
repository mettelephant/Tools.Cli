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

    public static void Log(LogEntry entry) => Instance?.Log(entry);

    public static void LogDebug(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = HelpExplanation.None)
        => Instance?.LogDebug(message, file, data, sourceExtension, destinationExtension, helpExplanation);
    public static void LogInformation(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = HelpExplanation.None)
        => Instance?.LogInformation(message, file, data, sourceExtension, destinationExtension, helpExplanation);
    public static void LogSummary(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = HelpExplanation.None)
        => Instance?.LogSummary(message, file, data, sourceExtension, destinationExtension, helpExplanation);
    public static void LogWarning(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = HelpExplanation.None)
        => Instance?.LogDebug(message, file, data, sourceExtension, destinationExtension, helpExplanation);
    public static void LogError(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = HelpExplanation.None)
        => Instance?.LogError(message, file, data, sourceExtension, destinationExtension, helpExplanation);
    public static void StartTask(string taskName) => Instance?.StartTask(taskName);
    public static void ReportProgress(string taskName, int progress) => Instance?.ReportProgress(taskName, progress);
    public static void CompleteTask(string taskName) => Instance?.CompleteTask(taskName);
    public static void LogSummary(string customerCode, int filesProcessed, int errors, int filesWithErrors, int filesMoved)
    {
        var summary = new SummaryEntry
        {
            CustomerCode = customerCode,
            FilesProcessed = filesProcessed,
            Errors = errors,
            FilesWithErrors = filesWithErrors,
            FilesMoved = filesMoved
        };
        Instance?.LogSummary(summary);
    }
}
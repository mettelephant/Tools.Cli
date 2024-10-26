using Tools.Cli.Logging.Help;

namespace Tools.Cli.Logging;

public sealed class NullLogger : ILogger
{
    private NullLogger()
    {
    }

    public static ILogger Instance { get; } = new NullLogger();

    public void Log(LogEntry logEntry)
    {
    }

    public void Log(LogLevel level, string message, FileInfo? file = null, string? data = null, string? sourceExtension = null,
        string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
    }

    public void LogDebug(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null,
        string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
    }

    public void LogInformation(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null,
        string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
    }

    public void LogWarning(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null,
        string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
    }

    public void LogSummary(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null,
        string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
    }

    public void LogError(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null,
        string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
    }

    public void StartTask(string taskName, double maxValue = 100)
    {
    }

    public void ReportProgress(string taskName, double value)
    {
    }

    public void CompleteTask(string taskName)
    {
    }

    public void LogSummary(SummaryEntry summary)
    {

    }
}
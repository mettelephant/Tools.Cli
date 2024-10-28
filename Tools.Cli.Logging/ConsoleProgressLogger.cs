using System.Collections.Concurrent;
using Spectre.Console;
using Tools.Cli.Logging.Help;

namespace Tools.Cli.Logging;

public class ConsoleProgressLogger(ProgressContext progressContext) : ILogger
{
    private readonly ConcurrentDictionary<string, ProgressTask> _tasks = new();

    public void LogDebug(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        Log(LogLevel.Debug, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogInformation(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        Log(LogLevel.Information, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogSummary(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        Log(LogLevel.Summary, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogWarning(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        Log(LogLevel.Warning, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogError(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        Log(LogLevel.Error, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void Log(LogEntry logEntry)
    {
        Log(logEntry.Level, logEntry.Message, logEntry.File, logEntry.Data, logEntry.SourceExtension, logEntry.DestinationExtension, logEntry.HelpExplanation);
    }

    public void Log(LogLevel level, string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        var levelColor = level switch
        {
            LogLevel.Debug => Color.Teal,
            LogLevel.Information => Color.Silver,
            LogLevel.Summary => Color.White,
            LogLevel.Warning => Color.Yellow,
            LogLevel.Error => Color.Maroon,
            _ => Color.LightSlateGrey
        };
        var messageColor = level switch
        {
            LogLevel.Debug => Color.DarkCyan,
            LogLevel.Information => Color.Grey,
            LogLevel.Summary => Color.White,
            LogLevel.Warning => Color.Olive,
            LogLevel.Error => Color.Red,
            _ => Color.LightSlateGrey
        };
        AnsiConsole.MarkupLine($"[{levelColor}]{level}[/][{messageColor}]: {message}[/]");
    }

    public void StartTask(string taskName, double maxValue = 100)
    {
        var task = progressContext.AddTask($"[{Color.Green}]{taskName}[/]", maxValue: maxValue);
        _tasks[taskName] = task;
    }

    public void ReportProgress(string taskName, double value)
    {
        if (_tasks.TryGetValue(taskName, out var task))
        {
            task.Increment(value);
        }
    }

    public void CompleteTask(string taskName)
    {
        if (_tasks.TryGetValue(taskName, out var task))
        {
            task.Increment(task.MaxValue);
        }
    }

    public void LogSummary(SummaryEntry summary)
    {
        var table = new Table()
            .Title("Summary")
            .AddColumn("Customer Code")
            .AddColumn("Files Processed")
            .AddColumn("Errors")
            .AddColumn("Files with Errors")
            .AddColumn("Files Moved")
            .AddRow(
                summary.CustomerCode,
                summary.FilesProcessed.ToString(),
                summary.Errors.ToString(),
                summary.FilesWithErrors.ToString(),
                summary.FilesMoved.ToString());
        AnsiConsole.Write(table);
    }
}
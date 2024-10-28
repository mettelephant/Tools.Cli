using System.Globalization;
using System.Text.Json;
using Tools.Cli.Logging.Help;

namespace Tools.Cli.Logging.File;

public class FileLogger : ILogger
{
    private readonly string _jsonLogPath;
    private readonly string _csvLogPath;
    private readonly string _summaryLogPath;

    public FileLogger(DirectoryInfo logDirectory)
    {
        // Ensure the directory exists
        if (!logDirectory.Exists)
        {
            logDirectory.Create();
        }
        _jsonLogPath = Path.Combine(logDirectory.FullName, "log.json");
        _csvLogPath = Path.Combine(logDirectory.FullName, "log.csv");
        _summaryLogPath = Path.Combine(logDirectory.FullName, "summary.json");
    }

    public void LogDebug(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        WriteLog(LogLevel.Debug, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogInformation(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        WriteLog(LogLevel.Information, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogWarning(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        WriteLog(LogLevel.Warning, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogSummary(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        WriteLog(LogLevel.Summary, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void LogError(string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        WriteLog(LogLevel.Error, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }
    public void Log(LogEntry logEntry)
    {
        WriteLog(logEntry.Level, logEntry.Message, logEntry.File, logEntry.Data, logEntry.SourceExtension, logEntry.DestinationExtension, logEntry.HelpExplanation);
    }

    public void Log(LogLevel level, string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        WriteLog(level, message, file, data, sourceExtension, destinationExtension, helpExplanation);
    }

    public void StartTask(string taskName, double maxValue = 100) { /* No-op for file logging */ }

    public void ReportProgress(string taskName, double value) { /* No-op for file logging */ }

    public void CompleteTask(string taskName) { /* No-op for file logging */ }

    private void WriteLog(LogLevel level, string message, FileInfo? file = null, string? data = null, string? sourceExtension = null, string? destinationExtension = null, HelpExplanation? helpExplanation = null)
    {
        var logData = new LogJsonEntry(DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture), level, message, file, data, sourceExtension, destinationExtension, helpExplanation);

        // Serialize to JSON
        var jsonLogEntry = JsonSerializer.Serialize(logData, SourceGenerationContext.Default.LogJsonEntry);

        // Write to JSON file
        System.IO.File.AppendAllText(_jsonLogPath, jsonLogEntry + Environment.NewLine);

        // Write to CSV file
        var csvLine = $"{logData.Timestamp},{logData.Level},\"{logData.Message}\",\"{logData.File}\",\"{logData.Data}\",\"{logData.SourceExtension}\",\"{logData.DestinationExtension}\",\"{logData.HelpExplanation}\"";
        System.IO.File.AppendAllText(_csvLogPath, $"{csvLine}{Environment.NewLine}");
    }

    public void LogSummary(SummaryEntry summary)
    {
        // Serialize to JSON
        var jsonSummaryEntry = JsonSerializer.Serialize(summary, SourceGenerationContext.Default.SummaryEntry);

        // Write to summary JSON file
        System.IO.File.WriteAllText(_summaryLogPath, jsonSummaryEntry);
    }
}

public record LogJsonEntry(string Timestamp, LogLevel Level, string Message, FileInfo? File, string? Data, string? SourceExtension, string? DestinationExtension, HelpExplanation? HelpExplanation)
{
    public override string ToString()
    {
        return $"{{ Timestamp = {Timestamp}, Level = {Level}, Message = {Message}, File = {File}, Data = {Data}, SourceExtension = {SourceExtension}, DestinationExtension = {DestinationExtension}, HelpExplanation = {HelpExplanation} }}";
    }
}
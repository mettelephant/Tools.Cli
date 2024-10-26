using System.Globalization;
using System.Text.Json;
using Tools.Cli.Logging.Help;

namespace Tools.Cli.Logging;

public class FileLogger : ILogger
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    private readonly DirectoryInfo _logDirectory;

    public FileLogger(DirectoryInfo logDirectory)
    {
        _logDirectory = logDirectory;
        // Ensure the directory exists
        if (!_logDirectory.Exists)
        {
            _logDirectory.Create();
        }
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
        var logData = new
        {
            Timestamp = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture),
            Level = level,
            Message = message,
            File = file,
            Data = data,
            SourceExtension = sourceExtension,
            DestinationExtension = destinationExtension,
            HelpExplanation = helpExplanation
        };

        // Serialize to JSON
        var jsonLogEntry = JsonSerializer.Serialize(logData, _jsonOptions);

        // Write to JSON file
        var jsonLogPath = Path.Combine(_logDirectory.FullName, "log.json");
        File.AppendAllText(jsonLogPath, jsonLogEntry + Environment.NewLine);

        // Write to CSV file
        var csvLogPath = Path.Combine(_logDirectory.FullName, "log.csv");
        var csvLine = $"{logData.Timestamp},{logData.Level},\"{logData.Message}\",\"{logData.File}\",\"{logData.Data}\",\"{logData.SourceExtension}\",\"{logData.DestinationExtension}\",\"{logData.HelpExplanation}\"";
        File.AppendAllText(csvLogPath, $"{csvLine}{Environment.NewLine}");
    }
}
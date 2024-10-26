using Tools.Cli.Logging;
using Tools.Core.Settings;

namespace CodeMerger.Libraries;

public static class LogAnalyzer
{
    public static void DoIt(LogAnalyzerSettings settings)
    {
        // Implement your log analysis logic here
        Logger.StartTask("Printing Summary");
        Logger.LogSummary("Analyzing log with the following parameters:");
        Logger.LogSummary($"Log Directory: {settings.LogDirectory?.FullName ?? "NOT PROVIDED"}");
        Logger.LogSummary($"Log Level: {settings.LogLevel}");
        Logger.LogSummary($"Log Separator: {settings.Separator}");
        Thread.Sleep(1000);
        Logger.CompleteTask("Printing Summary");


        Logger.StartTask("Analyzing Logs");
        Thread.Sleep(1000);

        Logger.LogDebug("Debugging information");
        Logger.LogWarning("This is a warning");
        Logger.LogError("An error occurred");
        // Implement your log analysis logic here
        Logger.LogInformation($"Analyzing logs from {settings.Separator}");

        // Simulate work
        Thread.Sleep(1000);

        // Log messages at different levels
        Logger.LogDebug("Debugging information");
        Logger.LogWarning("This is a warning");
        Logger.LogError("An error occurred");

        Logger.CompleteTask("Analyzing Logs");
    }
}
using Tools.Cli.Logging;
using Tools.Core.Settings;

namespace CodeMerger.Libraries;

public static class CodeMerger
{
    public static void DoIt(MergeSettings settings)
    {
        // Implement your merging logic here
        Logger.StartTask("Printing Summary");
        Logger.LogSummary("Merging code with the following parameters:");
        Logger.LogSummary($"SVN Directory: {settings.SvnDirectory.FullName}");
        Logger.LogSummary($"Git Directory: {settings.GitDirectory.FullName}");
        Logger.LogSummary($"Customer Code: {settings.CustomerCode}");
        Logger.LogSummary($"Clean SVN: {settings.CleanSvn}");
        Logger.LogSummary($"Clean Git: {settings.CleanGit}");
        Logger.LogSummary($"Log Level: {settings.LogLevel}");
        Logger.LogSummary($"Log Directory: {settings.LogDirectory?.FullName ?? "Not specified"}");
        Thread.Sleep(1000);
        
        Logger.LogSummary("test-cust", 100, 3, 3, 50);

        for (int i = 0; i < 100; i++)
        {
            Logger.ReportProgress("Printing Summary", i);
            Thread.Sleep(100);
        }
        Logger.CompleteTask("Printing Summary");


        Logger.StartTask("Analyzing Logs");
        Thread.Sleep(1000);

        // Implement your log analysis logic here
        Logger.LogInformation($"Merging code from {settings.SvnDirectory.FullName} and {settings.GitDirectory.FullName}");

        // Simulate work
        Thread.Sleep(1000);

        // Log messages at different levels
        Logger.LogDebug("Debugging information");
        Logger.LogWarning("This is a warning");
        Logger.LogError("An error occurred");

        Logger.CompleteTask("Analyzing Logs");
    }
}
using Tools.Core.Settings;

namespace CodeMerger.Libraries;

public static class CodeMergerLogReporter
{
    public static void DoIt(LogsReportSettings settings)
    {
        // Implement your log report generation logic here
        Console.WriteLine("Generating log report with the following parameters:");
        Console.WriteLine($"Log Directory: {settings.LogDirectory?.FullName}");
        Console.WriteLine($"Log Level: {settings.LogLevel}");
        Console.WriteLine($"Reports Directory: {settings.ReportsDirectory}");
    }
}
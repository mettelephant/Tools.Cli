using Tools.Core.Settings;

namespace CodeMerger.Libraries;

public static class WorkerConfigChangeService
{
    public static void DoIt(WorkerConfigChangeSettings changeSettings)
    {
        // Implement your log report generation logic here
        Console.WriteLine("Analyzing Worker Config Changes:");
        Console.WriteLine($"Log Directory: {changeSettings.LogDirectory?.FullName}");
        Console.WriteLine($"Log Level: {changeSettings.LogLevel}");
        Console.WriteLine($"Reports Directory: {changeSettings.ReportsDirectory}");
    }
}
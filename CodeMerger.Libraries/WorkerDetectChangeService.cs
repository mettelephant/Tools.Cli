using Tools.Core.Settings;

namespace CodeMerger.Libraries;

public static class WorkerDetectChangeService
{
    public static void DoIt(WorkerDetectChangeSettings settings)
    {
        // Implement your log report generation logic here
        Console.WriteLine("Worker Detecting Changes:");
        Console.WriteLine($"Log Directory: {settings.LogDirectory?.FullName}");
        Console.WriteLine($"Log Level: {settings.LogLevel}");
        Console.WriteLine($"Reports Directory: {settings.ReportsDirectory}");
    }
}
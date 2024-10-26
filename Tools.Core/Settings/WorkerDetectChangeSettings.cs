namespace Tools.Core.Settings;

public record WorkerDetectChangeSettings : BaseSettings
{
    public DirectoryInfo ReportsDirectory { get; init; } = null!;
    // Add more properties if needed
}
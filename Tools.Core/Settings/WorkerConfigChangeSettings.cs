namespace Tools.Core.Settings;

public record WorkerConfigChangeSettings : BaseSettings
{
    public DirectoryInfo ReportsDirectory { get; init; } = null!;
    // Add more properties if needed
}
namespace Tools.Core.Settings;

public record LogsReportSettings : BaseSettings
{
    public DirectoryInfo ReportsDirectory { get; init; } = null!;
    // Add more properties if needed
}
using Tools.Cli.Logging;

namespace Tools.Core.Settings;

public abstract record BaseSettings
{
    public LogLevel? LogLevel { get; init; }
    public DirectoryInfo? LogDirectory { get; init; }
    public bool EnableConsoleLogging { get; init; }
}
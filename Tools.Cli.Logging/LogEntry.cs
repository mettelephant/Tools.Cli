using Tools.Cli.Logging.Help;

namespace Tools.Cli.Logging;

public record LogEntry
{
    public LogLevel Level { get; set; }
    public required string Message { get; set; }
    public FileInfo? File { get; set; }
    public string? Data { get; set; }
    public string? SourceExtension { get; set; }
    public string? DestinationExtension { get; set; }
    public HelpExplanation? HelpExplanation { get; set; }
}
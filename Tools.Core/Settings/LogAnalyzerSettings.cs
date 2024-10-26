namespace Tools.Core.Settings;

public record LogAnalyzerSettings : BaseSettings
{
    public string Separator { get; init; } = null!;
}
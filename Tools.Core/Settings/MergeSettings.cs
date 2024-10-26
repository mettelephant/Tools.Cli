namespace Tools.Core.Settings;

public record MergeSettings : BaseSettings
{
    public DirectoryInfo SvnDirectory { get; init; } = null!;
    public DirectoryInfo GitDirectory { get; init; } = null!;
    public string CustomerCode { get; init; } = null!;
    public bool CleanSvn { get; init; }
    public bool CleanGit { get; init; }
}

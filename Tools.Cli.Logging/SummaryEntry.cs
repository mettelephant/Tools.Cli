namespace Tools.Cli.Logging;

public record SummaryEntry
{
    public string CustomerCode { get; set; } = default!;
    public int FilesProcessed { get; set; }
    public int Errors { get; set; }
    public int FilesWithErrors { get; set; }
    public int FilesMoved { get; set; }
}
// Models.cs
using System.Text.Json.Serialization;

namespace TestDataGenerator;

public enum LogLevel
{
    Info,
    Warning,
    Error
}

public class LogResult
{
    public string Code { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LogLevel LogLevel { get; set; }

    public string Message { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string SourceFile { get; set; } = string.Empty;
    public string DestinationFile { get; set; } = string.Empty;
    public string HelpUrl { get; set; } = string.Empty;
}

public class Summary
{
    public int FilesProcessed { get; set; }
    public int SuccessfulOperations { get; set; }
    public int FailedOperations { get; set; }
    public int Warnings { get; set; }
}
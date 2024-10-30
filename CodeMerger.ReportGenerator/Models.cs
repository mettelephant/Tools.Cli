using System.Text.Json.Serialization;

namespace HtmlReportBuilder;

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

public class CustomerReport
{
    public string CustomerId { get; set; } = string.Empty;
    public Summary Summary { get; set; } = new();
    public List<LogResult> LogResults { get; set; } = [];
}

public class OverallReport
{
    public int TotalCustomers { get; set; }
    public int CustomersWithNoErrors { get; set; }
    public int CustomersWithErrors { get; set; }
    public List<CustomerReport> CustomerReports { get; set; } = [];
}
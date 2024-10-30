using System.Text.Json.Serialization;

namespace HtmlReportBuilder;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
[JsonSerializable(typeof(List<LogResult>))]
[JsonSerializable(typeof(Summary))]
[JsonSerializable(typeof(CustomerReport))]
[JsonSerializable(typeof(OverallReport))]
public partial class ReportJsonContext : JsonSerializerContext
{
}
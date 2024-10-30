using System.Text.Json;

namespace HtmlReportBuilder;

public static class JsonHelper
{
    public static Summary ParseSummary(string summaryFilePath)
    {
        try
        {
            var json = File.ReadAllText(summaryFilePath);
            return JsonSerializer.Deserialize<Summary>(json, ReportJsonContext.Default.Summary)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing summary file at {summaryFilePath}: {ex.Message}");
            return new Summary();
        }
    }

    public static List<LogResult> ParseLogResults(string logResultsFilePath)
    {
        try
        {
            var json = File.ReadAllText(logResultsFilePath);
            return JsonSerializer.Deserialize<List<LogResult>>(json, ReportJsonContext.Default.ListLogResult)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing log results file at {logResultsFilePath}: {ex.Message}");
            return [];
        }
    }
}
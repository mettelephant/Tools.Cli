using System.Text;
using System.Text.Json;

namespace HtmlReportBuilder;

public static class HtmlReportGenerator
{
    public static void GenerateHtmlReport(OverallReport overallReport, string outputPath)
    {
        string reportTemplate = HtmlTemplate.GetReportTemplate();

        // Replace overall summary placeholders
        reportTemplate = reportTemplate.Replace("{TotalCustomers}", overallReport.TotalCustomers.ToString())
            .Replace("{CustomersWithNoErrors}", overallReport.CustomersWithNoErrors.ToString())
            .Replace("{CustomersWithErrors}", overallReport.CustomersWithErrors.ToString());

        // Inside HtmlReportGenerator.GenerateHtmlReport(...)
        StringBuilder tableRowsBuilder = new StringBuilder();
        StringBuilder logDataBuilder = new StringBuilder();
        logDataBuilder.AppendLine("var customerLogs = {");

        foreach (var customer in overallReport.CustomerReports)
        {
            // Aggregate unique log codes for the customer
            var uniqueLogCodes = customer.LogResults.Select(lr => lr.Code).Distinct();
            string logCodes = string.Join(", ", uniqueLogCodes);
            
            // Calculate total errors
            int errorCount = customer.LogResults.Count(lr => lr.LogLevel == LogLevel.Error);
            
            // Serialize log results to JSON
            string logJson = JsonSerializer.Serialize(customer.LogResults, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            logJson = logJson.Replace("\\", "\\\\").Replace("\"", "\\\"");
            
            // Add to logDataBuilder
            logDataBuilder.AppendLine($"    '{customer.CustomerId}': [{logJson}],");
            
            // Add to tableRowsBuilder
            tableRowsBuilder.Append($@"
                <tr>
                    <td class='details-control'></td>
                    <td>{customer.CustomerId}</td>
                    <td>{customer.Summary.FilesProcessed}</td>
                    <td>{customer.Summary.SuccessfulOperations}</td>
                    <td>{customer.Summary.FailedOperations}</td>
                    <td>{customer.Summary.Warnings}</td>
                    <td>{errorCount}</td>
                    <td>{logCodes}</td>
                </tr>
                <tr class='child-row' style='display: none;'>
                    <td colspan='8'>
                        <div class='child-row-content'>
                            <!-- Log details will be injected here via JavaScript -->
                        </div>
                    </td>
                </tr>
            ");
        }

        logDataBuilder.AppendLine("};");

        // Replace placeholders in the HTML template
        reportTemplate = reportTemplate.Replace("{CustomerTableRows}", tableRowsBuilder.ToString());
        reportTemplate = reportTemplate.Replace("{CustomerLogs}", logDataBuilder.ToString());

        // Write to output file
        File.WriteAllText(outputPath, reportTemplate);
        Console.WriteLine($"HTML report generated at: {outputPath}");

    }
}
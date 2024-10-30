/*using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HtmlReportBuilder;

// Enums and Data Classes
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
    public string CustomerId { get; set; }
    public Summary Summary { get; set; }
    public List<LogResult> LogResults { get; set; } = new List<LogResult>();
}

public class OverallReport
{
    public int TotalCustomers { get; set; }
    public int CustomersWithNoErrors { get; set; }
    public int CustomersWithErrors { get; set; }
    public List<CustomerReport> CustomerReports { get; set; } = new List<CustomerReport>();
}
    
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

// JSON Helper
public static class JsonHelper
{
    public static Summary ParseSummary(string summaryFilePath)
    {
        var json = File.ReadAllText(summaryFilePath);
        return JsonConvert.DeserializeObject<Summary>(json);
    }

    public static List<LogResult> ParseLogResults(string logResultsFilePath)
    {
        var json = File.ReadAllText(logResultsFilePath);
        return JsonConvert.DeserializeObject<List<LogResult>>(json);
    }
}

// Report Aggregator
public static class ReportAggregator
{
    public static OverallReport AggregateReports(string logsDirectory)
    {
        var overallReport = new OverallReport();

        // Get all report directories (e.g., 2024_10_29-19-54)
        var reportDirectories = Directory.GetDirectories(logsDirectory);
        if (reportDirectories.Length == 0)
        {
            Console.WriteLine("No report directories found.");
            return overallReport;
        }

        foreach (var reportDir in reportDirectories)
        {
            var customerDirectories = Directory.GetDirectories(reportDir);
            overallReport.TotalCustomers += customerDirectories.Length;

            foreach (var customerDir in customerDirectories)
            {
                var customerId = Path.GetFileName(customerDir);
                var summaryPath = Path.Combine(customerDir, "summary.json");
                var logResultsPath = Path.Combine(customerDir, "code-merger.json");

                if (!File.Exists(summaryPath) || !File.Exists(logResultsPath))
                {
                    Console.WriteLine($"Missing files for customer {customerId}. Skipping.");
                    continue;
                }

                var summary = JsonHelper.ParseSummary(summaryPath);
                var logResults = JsonHelper.ParseLogResults(logResultsPath);

                var customerReport = new CustomerReport
                {
                    CustomerId = customerId,
                    Summary = summary,
                    LogResults = logResults
                };

                overallReport.CustomerReports.Add(customerReport);

                if (customerReport.Summary.FailedOperations > 0 || logResults.Exists(lr => lr.LogLevel == LogLevel.Error))
                {
                    overallReport.CustomersWithErrors++;
                }
                else
                {
                    overallReport.CustomersWithNoErrors++;
                }
            }
        }

        return overallReport;
    }
}

// HTML Template Classes
public static class HtmlTemplate
{
    public static string GetReportTemplate()
    {
        return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Comprehensive Operation Report</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; background-color: #f9f9f9; }
        h1 { color: #2c3e50; }
        h2 { color: #34495e; }
        .summary, .customer-summary { background-color: #fff; padding: 20px; margin-bottom: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        .summary p, .customer-summary p { margin: 5px 0; }
        .collapsible { background-color: #2980b9; color: white; cursor: pointer; padding: 10px; width: 100%; border: none; text-align: left; outline: none; font-size: 16px; border-radius: 5px; margin-top: 10px; }
        .collapsible:after { content: '\25BC'; float: right; }
        .collapsible.active:after { content: "\25B2"; }
            .content { padding: 0 15px; display: none; background-color: #ecf0f1; overflow: hidden; border-radius: 5px; margin-bottom: 10px; }
        .log-entry { padding: 10px; border-bottom: 1px solid #bdc3c7; }
        .log-entry:last-child { border-bottom: none; }
        .log-info { color: #2980b9; }
        .log-warning { color: #e67e22; }
        .log-error { color: #c0392b; }
        table { width: 100%; border-collapse: collapse; margin-top: 10px; }
        th, td { border: 1px solid #bdc3c7; padding: 8px; text-align: left; }
        th { background-color: #34495e; color: white; }
        a { color: #2980b9; text-decoration: none; }
        a:hover { text-decoration: underline; }
        </style>
            </head>
            <body>

            <h1>Comprehensive Operation Report</h1>

            <!-- Overall Summary Section -->
            <div class='summary'>
            <h2>Overall Summary</h2>
            <p><strong>Total Customers:</strong> {TotalCustomers}</p>
            <p><strong>Customers with No Errors:</strong> {CustomersWithNoErrors}</p>
            <p><strong>Customers with Errors:</strong> {CustomersWithErrors}</p>
            </div>

            <!-- Individual Customer Reports -->
        {CustomerReports}

        <script>
            // Collapsible functionality
            var coll = document.getElementsByClassName('collapsible');
        for (var i = 0; i < coll.length; i++) {
            coll[i].addEventListener('click', function() {
                this.classList.toggle('active');
                var content = this.nextElementSibling;
                if (content.style.display === 'block') {
                    content.style.display = 'none';
                } else {
                    content.style.display = 'block';
                }
            });
        }
        </script>

            </body>
            </html>";
    }

    public static string GetCustomerReportTemplate()
    {
        return @"
    <div class='customer-summary'>
        <h2>Customer: {CustomerId}</h2>
        <p><strong>Files Processed:</strong> {FilesProcessed}</p>
        <p><strong>Successful Operations:</strong> {SuccessfulOperations}</p>
        <p><strong>Failed Operations:</strong> {FailedOperations}</p>
        <p><strong>Warnings:</strong> {Warnings}</p>

        <!-- Log Sections -->
        {LogSections}
    </div>";
    }

    public static string GetLogSectionTemplate(string code, List<LogResult> logResults)
    {
        var logEntries = new System.Text.StringBuilder();
        foreach (var log in logResults)
        {
            string logClass = log.LogLevel switch
            {
                LogLevel.Info => "log-info",
                LogLevel.Warning => "log-warning",
                LogLevel.Error => "log-error",
                _ => ""
            };

            string icon = log.LogLevel switch
            {
                LogLevel.Info => "ℹ️",
                LogLevel.Warning => "⚠️",
                LogLevel.Error => "❌",
                _ => ""
            };

            logEntries.AppendLine($@"
                <div class='log-entry'>
                    <p class='{logClass}'><strong>{icon} {log.LogLevel}:</strong> {log.Message}</p>
                    <p><strong>Data:</strong> {log.Data}</p>
                    <p><strong>Source File:</strong> {log.SourceFile}</p>
                    <p><strong>Destination File:</strong> {log.DestinationFile}</p>
                    {(string.IsNullOrEmpty(log.HelpUrl) ? "" : $"<p><a href='{log.HelpUrl}' target='_blank'>Help</a></p>")}
                </div>");
        }

        return $@"
        <button class='collapsible'>{code} ({logResults.Count})</button>
        <div class='content'>
            {logEntries}
        </div>";
    }
}

// HTML Report Generator
public static class HtmlReportGenerator
{
    public static void GenerateHtmlReport(OverallReport overallReport, string outputPath)
    {
        string reportTemplate = HtmlTemplate.GetReportTemplate();

        // Replace overall summary placeholders
        reportTemplate = reportTemplate.Replace("{TotalCustomers}", overallReport.TotalCustomers.ToString())
            .Replace("{CustomersWithNoErrors}", overallReport.CustomersWithNoErrors.ToString())
            .Replace("{CustomersWithErrors}", overallReport.CustomersWithErrors.ToString());

        // Generate individual customer sections
        StringBuilder customerSectionsBuilder = new StringBuilder();

        foreach (var customer in overallReport.CustomerReports)
        {
            var customerTemplate = HtmlTemplate.GetCustomerReportTemplate();

            customerTemplate = customerTemplate.Replace("{CustomerId}", customer.CustomerId)
                .Replace("{FilesProcessed}", customer.Summary.FilesProcessed.ToString())
                .Replace("{SuccessfulOperations}", customer.Summary.SuccessfulOperations.ToString())
                .Replace("{FailedOperations}", customer.Summary.FailedOperations.ToString())
                .Replace("{Warnings}", customer.Summary.Warnings.ToString());

            // Group log results by Code
            var groupedLogs = customer.LogResults.GroupBy(lr => lr.Code)
                .OrderBy(g => g.Key);

            StringBuilder logSectionsBuilder = new StringBuilder();

            foreach (var group in groupedLogs)
            {
                string code = group.Key;
                var logs = group.ToList();

                logSectionsBuilder.Append(HtmlTemplate.GetLogSectionTemplate(code, logs));
            }

            customerTemplate = customerTemplate.Replace("{LogSections}", logSectionsBuilder.ToString());

            customerSectionsBuilder.Append(customerTemplate);
        }

        // Replace customer reports placeholder
        reportTemplate = reportTemplate.Replace("{CustomerReports}", customerSectionsBuilder.ToString());

        // Write to output file
        File.WriteAllText(outputPath, reportTemplate);

        Console.WriteLine($"HTML report generated at: {outputPath}");
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        // Define the Logs directory path
        // You can modify this path or make it a command-line argument
        string logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

        if (!Directory.Exists(logsDirectory))
        {
            Console.WriteLine($"Logs directory not found at: {logsDirectory}");
            return;
        }

        // Aggregate reports
        var overallReport = ReportAggregator.AggregateReports(logsDirectory);

        // Define output HTML path
        string outputHtmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Comprehensive_Report.html");

        // Generate HTML report
        HtmlReportGenerator.GenerateHtmlReport(overallReport, outputHtmlPath);
    }
}*/
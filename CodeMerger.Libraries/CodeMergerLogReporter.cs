using System.Text.Json;
using Tools.Cli.Logging;
using Tools.Cli.Logging.File;
using Tools.Core.Settings;

namespace CodeMerger.Libraries;

public static class CodeMergerLogReporter
{
    public static void DoIt(LogsReportSettings settings)
    {
        // Implement your log report generation logic here
        Console.WriteLine("Generating log report with the following parameters:");
        Console.WriteLine($"Log Directory: {settings.LogDirectory?.FullName}");
        Console.WriteLine($"Log Level: {settings.LogLevel}");
        Console.WriteLine($"Reports Directory: {settings.ReportsDirectory}");

        if(!settings.ReportsDirectory.Exists)
        {
            settings.ReportsDirectory.Create();
        }
        var reportFile = new FileInfo(Path.Combine(settings.ReportsDirectory.FullName, "log.html"));
        using var fs = File.Create(reportFile.FullName);
        fs.Close();
        GenerateHtmlReport(settings.LogDirectory!, new FileInfo(Path.Combine(settings.ReportsDirectory.FullName, "log.html")));
    }

    private static void GenerateHtmlReport(DirectoryInfo logDirectory, FileInfo outputHtmlFile)
    {
        var jsonLogPath = Path.Combine(logDirectory.FullName, "log.json");
        var summaryLogPath = Path.Combine(logDirectory.FullName, "summary.json");

        // Read the summary data
        var summaryJson = File.ReadAllText(summaryLogPath);
        var summaryData = JsonSerializer.Deserialize<SummaryEntry>(summaryJson, SourceGenerationContext.Default.SummaryEntry);

        // Read the log entries
        var logEntries = new List<LogJsonEntry>();
        foreach (var line in File.ReadLines(jsonLogPath))
        {
            var logData = JsonSerializer.Deserialize<LogJsonEntry>(line, SourceGenerationContext.Default.LogJsonEntry);
            logEntries.Add(logData);
        }

        // Group log entries by level
        var groupedLogs = logEntries.GroupBy(e => e.Level).ToDictionary(g => g.Key, g => g.ToList());

        // Generate HTML
        var htmlContent = BuildHtmlContent(summaryData, groupedLogs);

        // Write HTML to file
        File.WriteAllText(outputHtmlFile.FullName, htmlContent);
    }

    private static string BuildHtmlContent(SummaryEntry summary, Dictionary<LogLevel, List<LogJsonEntry>> groupedLogs)
    {
        var htmlBuilder = new System.Text.StringBuilder();

    // HTML Header
    htmlBuilder.AppendLine("<!DOCTYPE html>");
    htmlBuilder.AppendLine("<html lang='en'>");
    htmlBuilder.AppendLine("<head>");
    htmlBuilder.AppendLine("<meta charset='UTF-8'>");
    htmlBuilder.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>");
    htmlBuilder.AppendLine("<title>Code Merge Report</title>");
    // Bootstrap CSS
    htmlBuilder.AppendLine("<link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css'>");
    // Chart.js
    htmlBuilder.AppendLine("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>");
    htmlBuilder.AppendLine("</head>");
    htmlBuilder.AppendLine("<body>");
    htmlBuilder.AppendLine("<div class='container mt-5'>");

    // Page Title
    htmlBuilder.AppendFormat($"<h1 class='mb-4'>Code Merge Report: {summary.CustomerCode}</h1>");

    // Summary Section
    htmlBuilder.AppendLine("<h2>Summary</h2>");
    htmlBuilder.AppendLine("<canvas id='summaryChart' width='400' height='200'></canvas>");

    // Summary Data for Chart.js
    htmlBuilder.AppendLine("<script>");
    htmlBuilder.AppendLine("var ctx = document.getElementById('summaryChart').getContext('2d');");
    htmlBuilder.AppendLine("var summaryChart = new Chart(ctx, {");
    htmlBuilder.AppendLine("    type: 'bar',");
    htmlBuilder.AppendLine("    data: {");
    htmlBuilder.AppendLine("        labels: ['Files Processed', 'Errors', 'Files with Errors', 'Files Moved'],");
    htmlBuilder.AppendLine("        datasets: [{");
    htmlBuilder.AppendLine("            label: 'Summary',");
    htmlBuilder.AppendFormat($"            data: [{summary.FilesProcessed}, {summary.Errors}, {summary.FilesWithErrors}, {summary.FilesMoved}],");
    htmlBuilder.AppendLine("            backgroundColor: [");
    htmlBuilder.AppendLine("                'rgba(54, 162, 235, 0.2)',");
    htmlBuilder.AppendLine("                'rgba(255, 99, 132, 0.2)',");
    htmlBuilder.AppendLine("                'rgba(255, 206, 86, 0.2)',");
    htmlBuilder.AppendLine("                'rgba(75, 192, 192, 0.2)'");
    htmlBuilder.AppendLine("            ],");
    htmlBuilder.AppendLine("            borderColor: [");
    htmlBuilder.AppendLine("                'rgba(54, 162, 235, 1)',");
    htmlBuilder.AppendLine("                'rgba(255, 99, 132, 1)',");
    htmlBuilder.AppendLine("                'rgba(255, 206, 86, 1)',");
    htmlBuilder.AppendLine("                'rgba(75, 192, 192, 1)'");
    htmlBuilder.AppendLine("            ],");
    htmlBuilder.AppendLine("            borderWidth: 1");
    htmlBuilder.AppendLine("        }]");
    htmlBuilder.AppendLine("    },");
    htmlBuilder.AppendLine("    options: {");
    htmlBuilder.AppendLine("        scales: {");
    htmlBuilder.AppendLine("            yAxes: [{");
    htmlBuilder.AppendLine("                ticks: {");
    htmlBuilder.AppendLine("                    beginAtZero: true");
    htmlBuilder.AppendLine("                }");
    htmlBuilder.AppendLine("            }]");
    htmlBuilder.AppendLine("        }");
    htmlBuilder.AppendLine("    }");
    htmlBuilder.AppendLine("});");
    htmlBuilder.AppendLine("</script>");

    // Customer Code Display
    htmlBuilder.AppendFormat($"<p><strong>Customer Code:</strong> {summary.CustomerCode}</p>");

        // Collapsible Sections for Each Log Level
        foreach (var level in new[] { LogLevel.Debug, LogLevel.Information, LogLevel.Warning })
        {
            if (groupedLogs.TryGetValue(level, out var entries))
            {
                var levelId = level.ToString().ToLower();

                // Collapsible Button
                htmlBuilder.AppendFormat($"<p><a class='btn btn-primary' data-toggle='collapse' href='#{levelId}Logs' role='button' aria-expanded='false' aria-controls='{levelId}Logs'>{level} Logs</a></p>");

                // Collapsible Content
                htmlBuilder.AppendFormat($"<div class='collapse' id='{levelId}Logs'>");
                htmlBuilder.AppendLine("<div class='card card-body'>");

                // Table of Log Entries
                htmlBuilder.AppendLine("<table class='table table-striped'>");
                htmlBuilder.AppendLine("<thead class='thead-dark'>");
                htmlBuilder.AppendLine("<tr><th>Timestamp</th><th>Message</th><th>File</th><th>Data</th><th>Source Extension</th><th>Destination Extension</th><th>Helplink</th></tr>");
                htmlBuilder.AppendLine("</thead>");
                htmlBuilder.AppendLine("<tbody>");
                foreach (var entry in entries)
                {
                    htmlBuilder.AppendLine("<tr>");
                    htmlBuilder.AppendFormat($"<td>{entry.Timestamp}</td>");
                    htmlBuilder.AppendFormat($"<td>{entry.Message}</td>");
                    htmlBuilder.AppendFormat($"<td>{entry.File?.FullName}</td>");
                    htmlBuilder.AppendFormat($"<td>{entry.Data}</td>");
                    htmlBuilder.AppendFormat($"<td>{entry.SourceExtension}</td>");
                    htmlBuilder.AppendFormat($"<td>{entry.DestinationExtension}</td>");
                    htmlBuilder.AppendFormat($"<td>{entry.HelpExplanation}</td>");
                    htmlBuilder.AppendLine("</tr>");
                }
                htmlBuilder.AppendLine("</tbody>");
                htmlBuilder.AppendLine("</table>");

                htmlBuilder.AppendLine("</div>");
                htmlBuilder.AppendLine("</div>");
            }
        }

        // Close container div
        htmlBuilder.AppendLine("</div>");

        // Bootstrap JS and dependencies
        htmlBuilder.AppendLine("<script src='https://code.jquery.com/jquery-3.5.1.slim.min.js'></script>");
        htmlBuilder.AppendLine("<script src='https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js'></script>");
        htmlBuilder.AppendLine("<script src='https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js'></script>");

        // HTML Footer
        htmlBuilder.AppendLine("</body>");
        htmlBuilder.AppendLine("</html>");

        return htmlBuilder.ToString();
    }
}
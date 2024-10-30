namespace HtmlReportBuilder;

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
            Console.WriteLine($"Processing report directory: {reportDir}");
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

                if (customerReport.Summary.FailedOperations > 0 ||
                    customerReport.LogResults.Any(lr => lr.LogLevel == LogLevel.Error))
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
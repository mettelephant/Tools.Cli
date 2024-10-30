namespace HtmlReportBuilder;

class Program
{
    static void Main(string[] args)
    {
        // Parse command-line arguments for flexibility
        string logsDirectory;
        string outputHtmlPath;

        if (args.Length >= 2)
        {
            logsDirectory = args[0];
            outputHtmlPath = args[1];
        }
        else
        {
            // Default paths if arguments are not provided
            logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            outputHtmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Comprehensive_Report.html");
        }

        if (!Directory.Exists(logsDirectory))
        {
            Console.WriteLine($"Logs directory not found at: {logsDirectory}");
            return;
        }

        // Aggregate reports from the Logs directory
        var overallReport = ReportAggregator.AggregateReports(logsDirectory);

        // Generate HTML report
        HtmlReportGenerator.GenerateHtmlReport(overallReport, outputHtmlPath);
    }
}
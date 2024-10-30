using System.Text;

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
        try
        {
            File.WriteAllText(outputPath, reportTemplate);
            Console.WriteLine($"HTML report generated at: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing HTML report to {outputPath}: {ex.Message}");
        }
    }
}